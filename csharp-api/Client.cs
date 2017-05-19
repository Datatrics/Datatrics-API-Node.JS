using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Datatrics.Module;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Datatrics
{
    /// <summary>
    /// Datatrics API Client
    /// </summary>
    public class Client
    {
        private const string CLIENT_VERSION = "2.0";
        
        /// <summary>
        /// API key for authentication
        /// </summary>
        public string ApiKey { get; private set; }
        /// <summary>
        /// Identifier of the project
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// API Version
        /// </summary>
        public string ApiVersion { get; set; } = "2.0";
        /// <summary>
        /// BaseModule url for the API
        /// </summary>
        public string ApiEndpoint { get; private set; } = "https://api.datatrics.com";
        /// <summary>
        /// ProfileModule module, used for profile api calls
        /// </summary>
        public ProfileModule Profile { get; private set; }
        /// <summary>
        /// Sales module, used for sales api calls
        /// </summary>
        public SaleModule Sale { get; private set; }
        /// <summary>
        /// ContentModule module, used for content api calls
        /// </summary>
        public ContentModule Content { get; private set; }

        private readonly HttpClient client;

        /// <summary>
        /// Client constructor requires a api key and project id
        /// </summary>
        /// <param name="apikey">Authentication key for the API</param>
        /// <param name="projectid">Project identifier</param>
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
            client.DefaultRequestHeaders.Add("x-apikey", ApiKey);

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
            HttpResponseMessage response = await client.PostAsync("/" + ApiVersion + url , new StringContent(data.ToString(Formatting.None)));

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
            Profile = new ProfileModule(this);
            Sale = new SaleModule(this);
            Content = new ContentModule(this);
        }
    }
}
