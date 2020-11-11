using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.QueueCore
{
    using System.Collections.Generic;

    public class CreateQueueParameters
    {
        private bool durableExchange = true;
        private bool persistentMessages = false;
        public Dictionary<string, object> Arguments
        {
            get;
            set;
        }
        public bool DurableExchange
        {
            get
            {
                return durableExchange;
            }
            set
            {
                durableExchange = value;
            }
        }
        public string ExchangeName
        {
            get;
            set;
        }
        public string ExchangeType
        {
            get;
            set;
        }
        public bool PersistentMessages
        {
            get
            {
                return persistentMessages;
            }
            set
            {
                persistentMessages = value;
            }
        }
        public ushort PrefetchCount
        {
            get;
            set;
        }
        public string QueueName
        {
            get;
            set;
        }
        public string RoutingKey
        {
            get;
            set;
        }
        public string Serialization
        {
            get;
            set;
        }
        public CreateQueueParameters()
        {
        }
    }
}
