using System;
using System.Collections.Generic;
using System.Text;

namespace QueueMQ.QueueCore
{
    using Newtonsoft.Json;
    using System.Text;

    internal class JsonSerializer : ISerializer
    {
        public JsonSerializer()
        {
        }

        public object Deserialize(byte[] serialized)
        {
            return ((serialized == null ? false : serialized.Length != 0) ? this.JsonToObject(serialized) : null);
        }

        private object JsonToObject(byte[] serialized)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(serialized));
        }

        private byte[] ObjectToJson(object obj)
        {
            byte[] bytes;
            if (!(obj is string))
            {
                string str = JsonConvert.SerializeObject(obj, Formatting.None);
                bytes = Encoding.UTF8.GetBytes(str);
            }
            else
            {
                bytes = Encoding.UTF8.GetBytes(obj as string);
            }
            return bytes;
        }

        public byte[] Serialize(object obj)
        {
            byte[] json;
            if (obj != null)
            {
                json = this.ObjectToJson(obj);
            }
            else
            {
                json = null;
            }
            return json;
        }
    }
}
