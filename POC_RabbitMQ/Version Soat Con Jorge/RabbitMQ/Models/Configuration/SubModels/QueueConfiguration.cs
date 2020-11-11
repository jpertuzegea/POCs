using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Configuration.SubModels
{
    public class QueueConfiguration
    {
        public string QueueName;
        public int PreFetchCount;
        public int PreFetchCountFail;
        public int TimeRetry;
        public bool Exchange;
        public BackOffConfiguration BackOfConfiguration;
        public static QueueConfiguration Default
        {
            get
            {
                QueueConfiguration Temp = new QueueConfiguration();
                Temp.BackOfConfiguration = null;
                Temp.PreFetchCount = 1;
                Temp.PreFetchCountFail = 1;
                Temp.QueueName = String.Empty;
                Temp.TimeRetry = 0;
                Temp.Exchange = false;
                return Temp;
            }
        }
    }
}
