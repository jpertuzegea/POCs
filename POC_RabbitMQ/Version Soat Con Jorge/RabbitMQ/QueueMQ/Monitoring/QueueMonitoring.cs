using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.Monitoring
{ 
    using System.Collections.Generic;
    using System.Linq;
    using QueueMQ.Monitoring.Models;
    using QueueMQ.Helper;

    public class QueueMonitoring
    {
        public static bool SelectByQueueName(string queue)
        {
            QueueNamesMonitoring queueNames = (new QMJSONConfigManagerGeneric<QueueNamesMonitoring>("QueueConfig.json")).GetConfig();
            List<Models.QueueMonitoring> queues = RabbitServiceMonitoring.GetQueues();
            if (queues != null && queues.Count > 0)
            {
                if (queues.Where(x => x.Name.Equals(queue)).Count() > 0 && queueNames.Queues.Where(x => x.Equals(queue)).Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool SelectByQueuesNames(List<string> queuesList)
        {
            bool result = false;
            QueueNamesMonitoring queueNames = (new QMJSONConfigManagerGeneric<QueueNamesMonitoring>("QueueConfig.json")).GetConfig();
            List<Models.QueueMonitoring> queues = RabbitServiceMonitoring.GetQueues();
            if (queues != null && queues.Count > 0)
            {
                foreach (var queue in queuesList)
                {
                    if (queues.Where(x => x.Name.Equals(queue)).Count() > 0 && queueNames.Queues.Where(x => x.Equals(queue)).Count() > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return result;
        }

        public static List<QueueStatusMonitoring> VerifyQueueStatus(List<string> queuesList)
        {
            List<QueueStatusMonitoring> queueStatuses = new List<QueueStatusMonitoring>();
            QueueNamesMonitoring queueNames = (new QMJSONConfigManagerGeneric<QueueNamesMonitoring>("QueueConfig.json")).GetConfig();
            List<Models.QueueMonitoring> queues = RabbitServiceMonitoring.GetQueues();
            if (queues != null && queues.Count > 0)
            {
                foreach (var queue in queuesList)
                {
                    if (queues.Where(x => x.Name.Equals(queue)).Count() > 0 && queueNames.Queues.Where(x => x.Equals(queue)).Count() > 0)
                    {
                        queueStatuses.Add(new QueueStatusMonitoring { QueueName = queue, Status = true });
                    }
                    else
                    {
                        queueStatuses.Add(new QueueStatusMonitoring { QueueName = queue, Status = false });
                    }
                }
            }
            return queueStatuses;
        }
    }
}
