using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Datatrics.Module
{
    public class Content : Base
    {
        public override string url => "/project/" + client.ProjectId + "/content";

        internal Content(Client client) : base(client)
        {
        }

        /// <summary>
        /// Gets a list of content or a specific one
        /// </summary>
        /// <param name="contentid">Content id</param>
        /// <param name="args">Query arguments, limit is set to a default of 25</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Get(string contentid = null, Dictionary<string, object> args = null)
        {
            if (args == null)
                args = new Dictionary<string, object> { { "limit", "25" } };

            if (!args.ContainsKey("type"))
                args["type"] = "item";

            if (!String.IsNullOrEmpty(contentid))
                return await client.GetAsync(url + "/" + contentid, args);

            return await client.GetAsync(url, args);
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
        public async Task<JObject> Update(JObject content)
        {
            JToken token;

            if (!content.TryGetValue("contentid", StringComparison.CurrentCulture, out token))
                throw new Exception("Content must contain a contentid");

            if (!content.TryGetValue("source", StringComparison.CurrentCulture, out token))
                throw new Exception("Content must contain a source");

            if (!content.TryGetValue("type", StringComparison.CurrentCulture, out token))
                throw new Exception("Content must contain a type");

            if ((string)content["type"] == "item")
                if (!content.TryGetValue("itemtype", StringComparison.CurrentCulture, out token))
                    throw new Exception("Content must contain a itemtype");

            return await client.PutAsync(url + "/" + (string)content["contentid"], content);
        }
    }
}
