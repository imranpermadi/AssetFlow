using FAMS_Models;
using FAMS_Models.Utilities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitReceiver;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using Topshelf;
using System.Configuration;

namespace FAMS_Service
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            var serviceName = ConfigurationManager.AppSettings["ServiceName"].ToString();


            HostFactory.Run(configure =>
            {
                configure.Service<ServiceInstaller>(service =>
                {
                    service.ConstructUsing(s => new ServiceInstaller());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.SetServiceName(serviceName);
                configure.SetDisplayName(serviceName);
                configure.SetDescription(serviceName);
            });
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            ConfigureService.Configure();
        }

      
    }
}
