using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Datatrics.Module;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Datatrics
{
    public class Client
    {
        private const string CLIENT_VERSION = "2.0";
        
        public string ApiKey { get; private set; }
        public int ProjectId { get; set; }
        public string ApiVersion { get; set; } = "2.0";
        public string ApiEndpoint { get; set; } = "https://api.datatrics.com";
        public Profile Profile { get; private set; }
        public Sale Sale { get; private set; }
        public Content Content { get; private set; }

        private readonly HttpClient client;

        public Client(string apikey, int projectid)
        {
            ApiKey = apikey;
            ProjectId = projectid;
            client = new HttpClient()
            {
                BaseAddress = new Uri(ApiEndpoint)
            };

            client.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _RegisterModules();
        }

        internal async System.Threading.Tasks.Task<JObject> GetAsync(string url, Dictionary<string, object> args)
        {
            args.Add("apikey", ApiKey);

            HttpResponseMessage response = await client.GetAsync("/" + ApiVersion + url + args.AsQueryString());
            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }

        internal async System.Threading.Tasks.Task<JObject> PostAsync(string url, JObject data)
        {
            HttpResponseMessage response = await client.PostAsync("/" + ApiVersion + url + "?apikey" + ApiKey, new StringContent(data.ToString(Formatting.None)));
            
            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }

        internal async System.Threading.Tasks.Task<JObject> PutAsync(string url, JObject data)
        {
            HttpResponseMessage response = await client.PutAsync("/" + ApiVersion + url + "?apikey" + ApiKey, new StringContent(data.ToString(Formatting.None)));

            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }

        internal async System.Threading.Tasks.Task<JObject> DeleteAsync(string url, Dictionary<string, object> args = null)
        {
            if (args == null)
                args = new Dictionary<string, object>();

            args.Add("apikey", ApiKey);

            HttpResponseMessage response = await client.DeleteAsync("/" + ApiVersion + url + args.AsQueryString());

            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }

        private void _RegisterModules()
        {
            Profile = new Profile(this);
            Sale = new Sale(this);
            Content = new Content(this);
        }
    }
}
