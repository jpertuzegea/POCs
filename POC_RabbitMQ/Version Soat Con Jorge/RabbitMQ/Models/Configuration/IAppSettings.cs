using Models.Configuration.SubModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Models.Configuration
{
    public interface IAppSettings
    {
        [JsonConverter(typeof(JsonGenericDictionaryOrArrayConverter))]
        Dictionary<string, object> AppKeys { get; set; }
        //LoggingConfiguration Logging { get; set; }
        //DataBaseAccess DataBaseAccess { get; set; }
        ConnectionMQ[] QueueAccess { get; set; }
    }
}
