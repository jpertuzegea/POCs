using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.Process.Interfaces;
using QueueMQ.Helper;
using QueueMQ.Model;
using RabbitMQ.Client; 

namespace QueueMQ.QueueCore
{
    public class MessageHandler<T> where T : IInputModel, new()
    {
        private Func<T, IDictionary<string, object>, IOutputModel> messageAction;
        private Action<ulong> AckMessage;
        private Action<ulong> NackMessage;
        private bool AutoAck;
        ulong DeliveryTag;
        private string RoutingKey;
        private T Message;
        public bool Complete;
        IBasicProperties BasicProperties;
        public MessageHandler(
            Func<T, IDictionary<string, object>, IOutputModel> messageAction,
            Action<ulong> AckMessage,
            Action<ulong> NackMessage,
            bool AutoAck,
            T Message,
            string RoutingKey,
            ulong DeliveryTag,
            IBasicProperties BasicProperties)
        {
            this.messageAction = messageAction;
            this.AckMessage = AckMessage;
            this.NackMessage = NackMessage;
            this.AutoAck = AutoAck;
            this.Complete = false;
            this.Message = Message;
            this.RoutingKey = RoutingKey;
            this.DeliveryTag = DeliveryTag;
            this.BasicProperties = BasicProperties;
        }

        public Task<MessageHandler<T>> RunMessageHandler()
        {
            return new Task<MessageHandler<T>>(() =>
            {
                try
                {
                    this.messageAction.Invoke(Message, BasicProperties.Headers);
                    if (!AutoAck)
                    {
                        AckMessage.Invoke(DeliveryTag);
                    }
                }
                catch (Exception Ex)
                {
                    NackMessage.Invoke(DeliveryTag);
                    QMLogHelper.SingleInstance.Register(new QueueManagerLogEntry("Ejecutando delegado en hilo de procesamiento", "QueueManager.dll", "Error en ejecución de delegado dentro de hilo de procesmaiento de mensaje", Message.ToString(), Ex), System.Diagnostics.EventLogEntryType.Error);
                }
                finally
                {
                    this.Complete = true;
                }
                return this;
            });
        }
    }
}
