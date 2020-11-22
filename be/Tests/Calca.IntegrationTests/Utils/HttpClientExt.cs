using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calca.IntegrationTests.Utils
{
    public static class HttpClientExt
    {
        public static Task<HttpResponseMessage> PostJson<TObject>(
            this HttpClient http, string uri, TObject obj)
        {
            return http.SendJson(uri, HttpMethod.Post, obj);
        }

        public static Task<HttpResponseMessage> PutJson<TObject>(
            this HttpClient http, string uri, TObject obj)
        {
            return http.SendJson(uri, HttpMethod.Put, obj);
        }

        private static Task<HttpResponseMessage> SendJson<TObject>(this HttpClient http, string uri, HttpMethod method, TObject obj)
        {
            var jsonStr = JsonSerializer.Serialize(obj);
            var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(method, uri)
            {
                Content = content
            };
            return http.SendAsync(request);
        }

        public static async Task<TObject> GetJsonObject<TObject>(this HttpClient http, TObject sample, string uri)
        {
            var jsonStr = await http.GetStringAsync(uri);
            var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<TObject>(jsonStr);
            return deserialized;
        }
    }
}
