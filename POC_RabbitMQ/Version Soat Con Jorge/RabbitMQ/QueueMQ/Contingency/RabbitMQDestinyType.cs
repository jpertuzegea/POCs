using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.Contingency
{    public sealed class RabbitMQDestinyType
    {
        public const string _EXCHANGE = "EXCHANGE";
        public const string _QUEUE = "QUEUE";
        public enum Type
        {
            _EXCHANGE,
            _QUEUE
        }
        public RabbitMQDestinyType()
        {
        }
        public string Map(Type valor)
        {
            switch (valor)
            {
                case Type._EXCHANGE: return _EXCHANGE;
                case Type._QUEUE: return _QUEUE;
                default: return _QUEUE;
            }
        }
    }
}
