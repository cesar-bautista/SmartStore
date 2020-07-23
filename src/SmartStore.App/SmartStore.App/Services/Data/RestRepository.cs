using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SmartStore.App.Abstractions.Data;

namespace SmartStore.App.Services.Data
{
    public class RestRepository : IRestRepository
    {
        #region Properties
        private HttpClient HttpClient { get; }
        #endregion

        #region Constructors

        public RestRepository(string baseAddress = null, int timeOut = 0)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.Expect100Continue = false;

            HttpClient = new HttpClient();
            if (!string.IsNullOrWhiteSpace(baseAddress))
                SetBaseAddress(baseAddress);
            if(timeOut != 0)
                SetTimeout(timeOut);

            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion

        #region JsonSerializerSettings

        private JsonSerializerSettings _jsonSerializerSettings;

        private JsonSerializerSettings JsonSerializerSettings =>
            _jsonSerializerSettings ?? (_jsonSerializerSettings = CreateJsonSettings());

        private static JsonSerializerSettings CreateJsonSettings()
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                Formatting = Newtonsoft.Json.Formatting.None
            };
            jsonSettings.Converters.Add(new StringEnumConverter());
            return jsonSettings;
        }

        #endregion

        public IRestRepository SetBaseAddress(string baseAddress)
        {
            HttpClient.BaseAddress = new Uri(baseAddress.EndsWith("/") ? baseAddress : $"{baseAddress}/");
            return this;
        }

        public IRestRepository SetTimeout(int timeOut)
        {
            HttpClient.Timeout = TimeSpan.FromMinutes(timeOut);
            return this;
        }

        private string SerializeObject(object content)
        {
            return content != null ? JsonConvert.SerializeObject(content, JsonSerializerSettings) : null;
        }

        #region Verbs
        // GET
        public async Task<TResult> GetAsync<TResult>(string path, object content = null)
        {
            var json = SerializeObject(content);
            var result = await GetAsync(path, json);
            return JsonConvert.DeserializeObject<TResult>(result, JsonSerializerSettings);
        }

        public async Task<string> GetAsync(string path, string content = null)
        {
            return await SendRequestAsync(path, HttpMethod.Get, content);
        }

        // POST
        public async Task<TResult> PostAsync<TResult>(string path, object content)
        {
            var json = SerializeObject(content);
            return await PostAsync<TResult>(path, json);
        }

        public async Task<TResult> PostAsync<TResult>(string path, string content = null)
        {
            var json = await PostAsync(path, content);
            return JsonConvert.DeserializeObject<TResult>(json, JsonSerializerSettings);
        }

        public async Task<string> PostAsync(string path, string content = null)
        {
            return await SendRequestAsync(path, HttpMethod.Post, content);
        }

        // PUT
        public async Task<TResult> PutAsync<TResult>(string path, object content)
        {
            var json = SerializeObject(content);
            return await PutAsync<TResult>(path, json);
        }

        public async Task<TResult> PutAsync<TResult>(string path, string content = null)
        {
            var json = await PutAsync(path, content);
            return JsonConvert.DeserializeObject<TResult>(json, JsonSerializerSettings);
        }

        public async Task<string> PutAsync(string path, string content = null)
        {
            return await SendRequestAsync(path, HttpMethod.Put, content);
        }

        // DELETE
        public async Task<TResult> DeleteAsync<TResult>(string path)
        {
            var json = await DeleteAsync(path);
            return JsonConvert.DeserializeObject<TResult>(json, JsonSerializerSettings);
        }

        public async Task<string> DeleteAsync(string path)
        {
            return await SendRequestAsync(path, HttpMethod.Delete);
        }
        #endregion

        // SEND
        private async Task<string> SendRequestAsync(string path, HttpMethod method, string content = null)
        {
            using (var message = new HttpRequestMessage(method, path))
            {
                if (content != null)
                {
                    using (var httpContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json"))
                    {
                        message.Content = httpContent;
                        using (var response = await SendRequestAsync(message))
                        {
                            return await response.Content.ReadAsStringAsync();
                        }
                    }
                }

                using (var response = await SendRequestAsync(message))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage message)
        {
            var response = await HttpClient.SendAsync(message);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"{(int)response.StatusCode} {response.StatusCode}: {responseContent}");
        }

        #region Dispose
        ~RestRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                HttpClient?.Dispose();
            }
        }
        #endregion
    }
}
