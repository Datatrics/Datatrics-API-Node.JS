using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Datatrics.Module
{
    public class Profile : Base
    {
        public override string url => "/project/" + client.ProjectId + "/profile";

        internal Profile(Client c) : base(c)
        {
        }

        /// <summary>
        /// Gets a list of profiles or a specific one
        /// </summary>
        /// <param name="profileid">Profile id</param>
        /// <param name="args">Query arguments, limit is set to a default of 25</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Get(string profileid = null, Dictionary<string, object> args = null)
        {
            if (args == null)
                args = new Dictionary<string, object> {{"limit", "25"}};

            if (!String.IsNullOrEmpty(profileid))
                return await client.GetAsync(url + "/" + profileid, args);

            return await client.GetAsync(url, args);
        }

        /// <summary>
        /// Creates a profile
        /// </summary>
        /// <param name="profile">Profile object</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Create(JObject profile)
        {
            return await client.PostAsync(url, profile);
        }

        /// <summary>
        /// Deletes profile by id
        /// </summary>
        /// <param name="profileid">Profile id</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Delete(string profileid)
        {
            return await client.DeleteAsync(url + "/" + profileid);
        }

        /// <summary>
        /// Update a profile
        /// </summary>
        /// <param name="profile">Profile object containing profileid</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Update(JObject profile)
        {
            JToken token;

            if (!profile.TryGetValue("profileid", StringComparison.CurrentCulture, out token))
                throw new Exception("Profile must contain a profileid");

            return await client.PutAsync(url + "/" + (string)profile["profileid"], profile);
        }

        /// <summary>
        /// Updates a maximum of 50 profiles at a time
        /// </summary>
        /// <param name="profiles">JArray containing profiles with a maximum of 50</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Bulk(JArray profiles)
        {
            if (profiles.Count > 50)
                throw new Exception("Maximum of 50 profiles allowed at a time");

            return await client.PostAsync(url + "/bulk", new JObject {["items"] = profiles});
        }
    }
}
