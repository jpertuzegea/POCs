using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.QueueCore
{
    public class NoneSerializer : ISerializer
    {
        public NoneSerializer()
        {
        }

        public object Deserialize(byte[] serialized)
        {
            return Encoding.UTF8.GetString(serialized);
        }

        public byte[] Serialize(object obj)
        {
            return Encoding.UTF8.GetBytes(obj as string);
        }
    }
}
