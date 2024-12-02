using FAMS_Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitReceiver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.IO;
using System.ServiceProcess;
using System.Text;

namespace FAMS_Service
{

    public class ServiceInstaller
    {
        public static List<string> cancelTagList = new List<string>();
        public static List<string> queues = new List<string>()
        {
            FAMS_Models.Utilities.Constants.BackendModule.DOCUMENT,
            FAMS_Models.Utilities.Constants.BackendModule.APPROVAL,
        };

        public static IModel channel;

        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            factory.Ssl.Enabled = false;
            factory.Ssl.AcceptablePolicyErrors |= System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch;

            var connection = factory.CreateConnection();
            channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            var exchange = ConfigurationManager.AppSettings["RpcName"].ToString().ToLower() + "_" + ConfigurationManager.AppSettings["Environment"].ToString().ToLower();

            channel.ExchangeDeclare(exchange: exchange, type: "topic");

            channel.BasicQos(0, 1, true);

            foreach (var queue in queues)
            {
                //  channel.QueueDeclare(queue: queue, durable: false,
                //exclusive: false, autoDelete: false, arguments: null);

                var qName = channel.QueueDeclare().QueueName;

                channel.QueueBind(qName, exchange, queue + ".*");

                var tag = channel.BasicConsume(queue: qName,
                  autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] " + tag + " Awaiting RPC requests " + qName + ", exchange " + queue + ".*");

                cancelTagList.Add(tag);
            }


            consumer.Received += (model, ea) =>
            {
                ResultData response = null;

                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                var routingKey = ea.RoutingKey;

                Console.WriteLine(" [.] Received " + routingKey + " on " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                var logFolder = Directory.GetCurrentDirectory() + "\\Logs\\" + DateTime.Now.ToString("MMyyyy");

                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                var message = "";
                try
                {
                    message = Encoding.UTF8.GetString(body);

                    response = MessageHandler.HandleMessage(routingKey, message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                    response = null;
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                    channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                      basicProperties: replyProps, body: responseBytes);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag,
                      multiple: false);

                    File.WriteAllText(logFolder + $"\\{DateTime.Now.ToString("ddMMyyyy")}.txt", "\n" + " [.] Received " + routingKey + " on " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " => " + message);
                }
            };
        }

        public void Stop()
        {
            try
            {
                foreach (var tag in cancelTagList)
                {
                    channel.BasicCancel(tag);
                }

                foreach (var queue in queues)
                {
                    channel.QueueDelete(queue);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
