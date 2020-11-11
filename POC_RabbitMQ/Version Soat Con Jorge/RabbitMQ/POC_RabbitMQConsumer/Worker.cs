using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models.Configuration;
using Models.Configuration.SubModels;
using Models.Process.Interfaces;
using POC_RabbitMQConsumer.Bussines;
using POC_RabbitMQConsumer.Enums;
using POC_RabbitMQConsumer.Models;
using QueueMQ;
using QueueMQ.Helper;
using QueueMQ.QueueConsumer;
using Utilities.Helper;

namespace POC_RabbitMQConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static AppSettings _AppSettings;
        private ConnectionMQ _cmq;
        private QueueConfiguration NotificaionQueue = null;

        private BLL_Poc BLL_Poc = null;

        public static AppSettings AppSettings
        {
            get
            {
                if (_AppSettings == null)
                {
                    _AppSettings = new AppSettings();
                }
                return _AppSettings;
            }
            set
            {
                _AppSettings = value;
            }
        }


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            AppSettings = HelperConfiguration<AppSettings>.JSONConfigManagerGeneric();
            AppSettings.QueueAccess.Select(p =>
            {
                QueueHelper.Configuration = p;
                return true;
            }
            ).ToList();
            _logger = logger;
            _cmq = AppSettings.QueueAccess.FirstOrDefault(c => c.Name == "Principal");
            // LogHelper.SingleInstance.SetConfiguration(AppSettings.EventViewer);
            QMLogHelper.SingleInstance.SetConfiguration(AppSettings.EventViewer);

            NotificaionQueue = _cmq.QueueConfigurations.Where(q => q.QueueName == EnumQueue.PocJorgeQueue.ToString()).First();

            // Instancias de Business
            BLL_Poc = new BLL_Poc();

        }






        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                new TemplateQueue<Persona>(_cmq, NotificaionQueue, ExecuteQueue).TransactionalSubscribe();
            });

        }


        private IOutputModel ExecuteQueue(IInputModel arg, IDictionary<string, object> properties)
        {
            var model = (Persona)arg;
            BLL_Poc.Procesar(model);
            return model;
        }

    }
}
