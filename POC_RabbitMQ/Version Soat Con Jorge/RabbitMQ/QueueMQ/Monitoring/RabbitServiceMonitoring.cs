

namespace QueueMQ.Monitoring
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using QueueMQ.Monitoring.Models;
    using QueueMQ.Helper;

    internal static class RabbitServiceMonitoring
    {
        //UrlService url = (new JSONConfigManagerGeneric<UrlService>("QueueManagerConfig.json")).GetConfig();

        public static List<ExchangeMonitoring> GetExchanges()
        {
            UrlServiceMonitoring url = (new QMJSONConfigManagerGeneric<UrlServiceMonitoring>("QueueConfig.json")).GetConfig();
            ServiceMonitoring service = (new QMJSONConfigManagerGeneric<ServiceMonitoring>("QueueConfig.json")).GetConfig();
            string urlApi = @"" + url.RabbitURL + service.GetExhanges;
            List<ExchangeMonitoring> exchanges = JsonConvert.DeserializeObject<List<ExchangeMonitoring>>(ResponseMessage(urlApi));

            return exchanges;
        }

        public static List<Models.QueueMonitoring> GetQueues()
        {
            UrlServiceMonitoring url = (new QMJSONConfigManagerGeneric<UrlServiceMonitoring>("QueueConfig.json")).GetConfig();
            ServiceMonitoring service = (new QMJSONConfigManagerGeneric<ServiceMonitoring>("QueueConfig.json")).GetConfig();
            string urlApi = @"" + url.RabbitURL + service.GetQueue;
            List<Models.QueueMonitoring> queues = JsonConvert.DeserializeObject<List<Models.QueueMonitoring>>(ResponseMessage(urlApi));

            return queues;
        }

        public static List<BindingMonitoring> GetBindings()
        {
            UrlServiceMonitoring url = (new QMJSONConfigManagerGeneric<UrlServiceMonitoring>("QueueConfig.json")).GetConfig();
            ServiceMonitoring service = (new QMJSONConfigManagerGeneric<ServiceMonitoring>("QueueConfig.json")).GetConfig();
            string urlApi = @"" + url.RabbitURL + service.GetBindings;
            List<BindingMonitoring> bindings = JsonConvert.DeserializeObject<List<BindingMonitoring>>(ResponseMessage(urlApi));

            return bindings;
        }

        private static string ResponseMessage(string urlApi)
        {
            UrlServiceMonitoring url = (new QMJSONConfigManagerGeneric<UrlServiceMonitoring>("QueueConfig.json")).GetConfig();
            var proxiedHttpClientHandler = new HttpClientHandler() { UseProxy = true };
            proxiedHttpClientHandler.Proxy = new WebProxy(url.Proxy, url.Port);
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(url.UserName + ":" + url.Password));

            HttpClient client = new HttpClient(proxiedHttpClientHandler)
            {
                BaseAddress = new Uri(urlApi)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            HttpResponseMessage response = client.GetAsync(String.Empty).Result;

            return response.Content.ReadAsStringAsync().Result.ToString();
        }
    }
}
