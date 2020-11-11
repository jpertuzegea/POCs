using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.QueueCore
{
    internal interface ISerializer
    {
        object Deserialize(byte[] serialized);
        byte[] Serialize(object obj);
    }
}
