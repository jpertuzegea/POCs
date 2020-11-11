using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.Monitoring.Models
{
    public class QueueStatusMonitoring
    {
        public string QueueName { get; set; }
        public bool Status { get; set; }
    }
}
