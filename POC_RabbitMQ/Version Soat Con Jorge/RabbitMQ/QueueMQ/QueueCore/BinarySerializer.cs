using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.QueueCore
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    internal class BinarySerializer : ISerializer
    {
        public BinarySerializer()
        {
        }

        private object BinaryToObject(byte[] serialized)
        {
            object obj;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(serialized))
            {
                obj = binaryFormatter.Deserialize(memoryStream);
            }
            return obj;
        }

        public object Deserialize(byte[] serialized)
        {
            return ((serialized == null ? false : serialized.Length != 0) ? this.BinaryToObject(serialized) : null);
        }

        private byte[] ObjectToBinary(object obj)
        {
            byte[] array;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                array = memoryStream.ToArray();
            }
            return array;
        }

        public byte[] Serialize(object obj)
        {
            byte[] binary;
            if (obj != null)
            {
                binary = this.ObjectToBinary(obj);
            }
            else
            {
                binary = null;
            }
            return binary;
        }
    }
}
