using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Datatrics.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Datatrics.Module
{
    /// <summary>
    /// ContentModule module, used for content api calls
    /// </summary>
    public class ContentModule : BaseModule
    {
        /// <summary>
        /// Base url for this module
        /// </summary>
        public override string url => "/project/" + client.ProjectId + "/content";

        internal ContentModule(Client client) : base(client)
        {
        }

        /// <summary>
        /// Gets content by id and type
        /// </summary>
        /// <param name="contentid">Content id</param>
        /// <param name="type">Content type</param>
        /// <returns>Content objectt</returns>
        public async Task<Content> Get(string contentid, string type = "item")
        {
            Dictionary<string, object> args = new Dictionary<string, object> { {"type", type} };
            
            JObject response = await client.GetAsync(url + "/" + contentid, args);

            JToken token;
            if (!response.TryGetValue("id", out token))
                throw new Exception(response.ToString(Formatting.None));


            return new Content(response);
        }

        /// <summary>
        /// Gets a list of content
        /// </summary>
        /// <param name="args">Query arguments, limit is set to a default of 25</param>
        /// <returns>Result of the request</returns>
        public async Task<ListResult<Content>> GetList(Dictionary<string, object> args = null)
        {
            if (args == null)
                args = new Dictionary<string, object>();
            
            if (!args.ContainsKey("limit"))
                args.Add("limit", "25");

            if (!args.ContainsKey("type"))
                args.Add("type", "item");

            JObject response = await client.GetAsync(url, args);

            JToken token;
            if (!response.TryGetValue("total_elements", out token))
                throw new Exception(response.ToString(Formatting.None));

            return new ListResult<Content>(response);
        }

        /// <summary>
        /// Creates content
        /// </summary>
        /// <param name="content">Content object</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Create(Content content)
        {
            return await Create(content.ToJObject());
        }

        /// <summary>
        /// Creates content
        /// </summary>
        /// <param name="content">Content object</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Create(JObject content)
        {
            return await client.PostAsync(url, content);
        }

        /// <summary>
        /// Deletes content by id
        /// </summary>
        /// <param name="contentid">Content id</param>
        /// <param name="args">Query arguments</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Delete(string contentid, Dictionary<string, object> args = null)
        {
            if (args == null)
                args = new Dictionary<string, object>() { {"type", "item"}, {"itemtype", "product"}};

            if (!String.IsNullOrEmpty(contentid))
            {
                if (!args.ContainsKey("source"))
                    throw new Exception("Source is required");
                if (!args.ContainsKey("type"))
                    throw new Exception("Type is required");

                if ((string)args["type"] == "item")
                    if (!args.ContainsKey("itemtype"))
                        throw new Exception("Itemtype is required");
            }

            return await client.DeleteAsync(url + "/" + contentid, args);
        }

        /// <summary>
        /// Update content
        /// </summary>
        /// <param name="content">Content object</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Update(Content content)
        {
            return await Update(content.ToJObject());
        }

        /// <summary>
        /// Update content
        /// </summary>
        /// <param name="content">Content object</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Update(JObject content)
        {
            JToken token;

            if (!content.TryGetValue("contentid", StringComparison.CurrentCulture, out token))
                throw new Exception("ContentModule must contain a contentid");

            if (!content.TryGetValue("source", StringComparison.CurrentCulture, out token))
                throw new Exception("ContentModule must contain a source");

            if (!content.TryGetValue("type", StringComparison.CurrentCulture, out token))
                throw new Exception("ContentModule must contain a type");

            if ((string)content["type"] == "item")
                if (!content.TryGetValue("itemtype", StringComparison.CurrentCulture, out token))
                    throw new Exception("ContentModule must contain a itemtype");

            return await client.PutAsync(url + "/" + (string)content["contentid"], content);
        }

        /// <summary>
        /// Updates a maximum of 50 Content items at a time
        /// </summary>
        /// <param name="content">array content items with a maximum of 50 items</param>
        /// <param name="type">content type</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Bulk(Content[] content, string type = "items")
        {
            if (content.Length > 50)
                throw new Exception("Maximum of 50 content allowed at a time");

            JArray arr = new JArray();

            foreach (Content c in content)
                arr.Add(c.ToJObject());

            return await client.PostAsync(url + "/bulk", new JObject { ["items"] = arr, ["type"] = type });
        }

        /// <summary>
        /// Updates a maximum of 50 Content items at a time
        /// </summary>
        /// <param name="content">JArray containing content items with a maximum of 50 items</param>
        /// <param name="type">content type</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Bulk(JArray content, string type = "items")
        {
            if (content.Count > 50)
                throw new Exception("Maximum of 50 content allowed at a time");

            return await client.PostAsync(url + "/bulk", new JObject { ["items"] = content, ["type"] = type });
        }
    }
}
