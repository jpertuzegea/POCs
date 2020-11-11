using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.QueueCore
{
    public static class ExchangeType
    {
        public const string Direct = "direct";

        public const string Fanout = "fanout";

        public const string Headers = "headers";

        public const string Topic = "topic";
    }
}
