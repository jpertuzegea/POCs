using Models.Configuration.SubModels.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Configuration.SubModels
{
    public class EventViewerConfiguration
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EventViewerLevel Configuration { get; set; }
    }
}
