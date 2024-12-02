using FAMS_Data;
using FAMS_Models.Utilities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_API.Utilities
{
    public class RpcClient
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties props;

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            factory.Ssl.Enabled = false;
            factory.Ssl.AcceptablePolicyErrors |= System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch;

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            var exchange = ConfigurationManager.AppSettings["RpcName"].ToString().ToLower() + "_" + ConfigurationManager.AppSettings["Environment"].ToString().ToLower();

            channel.ExchangeDeclare(exchange: exchange, type: "topic");

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;


            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(response);
                }
            };

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);
        }

        public string Call(string routingKey, User user, object data, bool serializeData = true)
        {
            BackendModel model = new BackendModel();

            model.user = new UserData()
            {
                Username = user.Username,
                Fullname = user.Fullname,
                //Language = user.language
            };

            model.data = serializeData ? JsonConvert.SerializeObject(data) : data.ToString();

            var exchange = ConfigurationManager.AppSettings["RpcName"].ToString().ToLower() + "_" + ConfigurationManager.AppSettings["Environment"].ToString().ToLower();

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
            channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: props,
                body: messageBytes);

            return respQueue.Take();
        }

        public string CallPing(string routingKey, object data, bool serializeData = true)
        {
            BackendModel model = new BackendModel();

            model.data = serializeData ? JsonConvert.SerializeObject(data) : data.ToString();

            var exchange = ConfigurationManager.AppSettings["RpcName"].ToString().ToLower() + "_" + ConfigurationManager.AppSettings["Environment"].ToString().ToLower();

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
            channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: props,
                body: messageBytes);

            return respQueue.Take();
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
