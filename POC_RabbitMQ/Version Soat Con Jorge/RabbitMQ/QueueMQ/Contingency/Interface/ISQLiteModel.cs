using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.Contingency.Interface
{
    public interface ISQLiteModel
    {
        int id { get; set; }
        string TableName { get; set; }
        bool Save();
        bool Select(int id);
    }
}
