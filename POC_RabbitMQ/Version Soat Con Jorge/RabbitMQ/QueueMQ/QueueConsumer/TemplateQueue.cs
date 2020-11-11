
namespace QueueMQ.QueueConsumer
{
    using QueueMQ.Helper;
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Linq;
    using QueueMQ.Configuration;
    using QueueMQ.Model;
    using QueueMQ.Enum; 
    using System.Collections.Generic;
    using Models.Process.Interfaces;
    using Models.Configuration.SubModels;
    using Models.Result;
    using Models.Result.Enum;

    public class TemplateQueue<T> where T : IInputModel, new()
    {
        private QueueConfiguration _QUEUECONFIG;
        private ConnectionMQ _CONNECTIONCONFIG;
        Func<T, IDictionary<string, object>, IOutputModel> ProcessMessage;
        public TemplateQueue(ConnectionMQ ConnectionConfig, QueueConfiguration QueueConfig, Func<T, IDictionary<string, object>, IOutputModel> Process)
        {
            _QUEUECONFIG = QueueConfig;
            _CONNECTIONCONFIG = ConnectionConfig;
            ProcessMessage = Process;
            PrincipalQueeName = QueueConfig.QueueName;
        }

        protected ushort _PrefetchCount;

        protected ushort PrefetchCount
        {
            get
            {
                _PrefetchCount = (ushort)_QUEUECONFIG.PreFetchCount;
                return _PrefetchCount;
            }
            set { _PrefetchCount = value; }
        }

        protected ushort _PrefetchCountFail;

        protected ushort PrefetchCountFail
        {
            get
            {
                _PrefetchCountFail = (ushort)_QUEUECONFIG.PreFetchCountFail;
                return _PrefetchCountFail;
            }
            set { _PrefetchCountFail = value; }
        }

        protected string _PrincipalQueeName;

        protected string PrincipalQueeName
        {
            get
            {
                return _PrincipalQueeName;
            }
            set
            {
                _PrincipalQueeName = value;
            }
        }

        private string _FailedQueAsigned;

        public string FailedQueAsigned
        {
            get
            {
                if (string.IsNullOrEmpty(_FailedQueAsigned))
                {
                    _FailedQueAsigned = string.Format("Fail{0}", PrincipalQueeName);
                }
                return _FailedQueAsigned;
            }
            set { _FailedQueAsigned = value; }
        }

        public IResult<bool, ErrorModel> TransactionalSubscribe()
        {
            try
            {
                ushort prefetchCount = PrincipalQueeName.Contains("Fail") ? PrefetchCountFail : PrefetchCount;
                QueueInstance<T>.GetQueueInstance(_CONNECTIONCONFIG, _QUEUECONFIG).SubscribeToQueue((messageBody, header) =>
                {
                    try
                    {
                        return ProcessMessage.Invoke(messageBody, header);
                    }
                    catch (Exception Ex)
                    {
                        QMLogHelper.SingleInstance.Register(new QueueManagerLogEntry("Ejecutando delegado de procesamiento de mensaje", "QueueManager.dll", Ex.Message, String.Empty, Ex), EventLogEntryType.Error, QMLogName.QMRabbitMQ);
                    }
                    return null;
                }, noAck: false);
                return new ResultValue<bool, ErrorModel>(true);
            }
            catch (Exception Ex)
            {
                QMLogHelper.SingleInstance.Register(new QueueManagerLogEntry("Suscripción a cola", "QueueManager.dll", Ex.Message, String.Empty, Ex), EventLogEntryType.Information, QMLogName.QMRabbitMQ);
                return new ResultError<bool, ErrorModel>(ErrorModel.CreateErrorModel(
                    Ex.Message,
                    ErrorType.TechnicalFault,
                    Ex
                ));
            }
        }

        protected void CreateEventLog(object body, Exception e)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(string.Format("Error Queue: {0}", PrincipalQueeName));
            message.AppendLine(string.Format("Exception: {0}", e.Message));
            if (body != null)
            {
                int maxSixeLog = 30000;
                string dataObject = body.ToString();
                if (dataObject.Length > maxSixeLog)
                {
                    dataObject = dataObject.Substring(0, maxSixeLog);
                }
                message.AppendLine(Environment.NewLine);
                message.AppendLine(string.Format("Object: {0}", dataObject));
            }
            message.AppendLine(Environment.NewLine);
            message.AppendLine(string.Format("Date: {0}", DateTime.Now.ToString()));
            QMLogHelper.SingleInstance.Register(new QueueManagerLogEntry("Creando log de Eventos", "QueueManager.dll", "CreateEventLog", message.ToString(), null), System.Diagnostics.EventLogEntryType.Information, QMLogName.QMRabbitMQ);
        }
    }

}
