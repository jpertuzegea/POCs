
namespace QueueMQ.Helper
{

    using System;
    using System.IO;
    using System.Reflection;
    using Microsoft.CSharp.RuntimeBinder;

    public class QueueManagerHelperConfiguration<T>
    {
        public static T GetAppSettings(string ConfigurationFile = "appsettings.json", string configurationSection = "AppSettings")
        {
            //T Config;
            string TextData = string.Empty;
            dynamic ConfigTmp;
            if (System.IO.File.Exists(ConfigurationFile))
            {
                TextData = System.IO.File.ReadAllText(ConfigurationFile);
            }
            else
            {
                string temporalPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + ConfigurationFile.TrimStart('\\');
                if (System.IO.File.Exists(temporalPath))
                {
                    TextData = System.IO.File.ReadAllText(temporalPath);
                }
                else
                {
                    return default(T);
                }
            }
            ConfigTmp = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(TextData);
            try
            {
                string test = Newtonsoft.Json.JsonConvert.SerializeObject(ConfigTmp[configurationSection]);
                T Value = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(test);
                return Value;
            }
#pragma warning disable CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            catch (Exception Ex)
            {
#pragma warning restore CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
                //throw Ex;
                return default(T);
            }
        }
        public static T JSONConfigManagerGeneric(string ConfigName = "appsettings.json")
        {
            T Config;
            if (System.IO.File.Exists(ConfigName))
            {
                Config = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(ConfigName));
            }
            else
            {
                string temporalPath = System.IO.File.ReadAllText(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + ConfigName.TrimStart('\\'));
                if (System.IO.File.Exists(temporalPath))
                {
                    Config = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(temporalPath));
                }
                else
                {
                    Config = default(T);
                }
            }
            return Config;
        }
    }

}
