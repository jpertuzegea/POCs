using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.QueueCore
{
    public static class ArgumentsNames
    {
        public const string MaxLength = "x-max-length";

        public const string DeadLetterExchange = "x-dead-letter-exchange";

        public const string MessageDeduplication = "x-message-deduplication";
    }
}
