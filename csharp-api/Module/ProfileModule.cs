using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Datatrics.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Datatrics.Module
{
    /// <summary>
    /// ProfileModule module, used for profile api calls
    /// </summary>
    public class ProfileModule : BaseModule
    {
        /// <summary>
        /// BaseModule url for this module
        /// </summary>
        public override string url => "/project/" + client.ProjectId + "/profile";

        internal ProfileModule(Client c) : base(c)
        {
        }

        /// <summary>
        /// Gets a profile by id
        /// </summary>
        /// <param name="profileid">ProfileModule id</param>
        /// <returns>Result of the request</returns>
        public async Task<Profile> Get(string profileid)
        {
            JObject response = await client.GetAsync(url + "/" + profileid, new Dictionary<string, object>());

            JToken token;
            if (!response.TryGetValue("profileid", out token))
                throw new Exception(response.ToString(Formatting.None));

            return new Profile(response);
        }

        /// <summary>
        /// Gets a list of profiles or a specific one
        /// </summary>
        /// <param name="args">Query arguments, limit is set to a default of 25</param>
        /// <returns>Result of the request</returns>
        public async Task<ListResult<Profile>> GetList(Dictionary<string, object> args = null)
        {
            if (args == null)
                args = new Dictionary<string, object> { { "limit", "25" } };

            JObject response = await client.GetAsync(url, args);

            JToken token;
            if (!response.TryGetValue("total_elements", out token))
                throw new Exception(response.ToString(Formatting.None));

            return new ListResult<Profile>(response);
        }

        /// <summary>
        /// Creates a profile
        /// </summary>
        /// <param name="profile">Profile object</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Create(Profile profile)
        {
            return await Create(profile.ToJObject());
        }

        /// <summary>
        /// Creates a profile
        /// </summary>
        /// <param name="profile">JObject containing the profile</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Create(JObject profile)
        {
            return await client.PostAsync(url, profile);
        }

        /// <summary>
        /// Deletes profile by id
        /// </summary>
        /// <param name="profileid">ProfileModule id</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Delete(string profileid)
        {
            return await client.DeleteAsync(url + "/" + profileid);
        }

        /// <summary>
        /// Update a profile
        /// </summary>
        /// <param name="profile">Profile object</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Update(Profile profile)
        {
            return await Update(profile.ToJObject());
        }

        /// <summary>
        /// Update a profile
        /// </summary>
        /// <param name="profile">JObject containing the profile object</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Update(JObject profile)
        {
            JToken token;

            if (!profile.TryGetValue("profileid", StringComparison.CurrentCulture, out token))
                throw new Exception("ProfileModule must contain a profileid");

            return await client.PutAsync(url + "/" + (string)profile["profileid"], profile);
        }

        /// <summary>
        /// Updates a maximum of 50 profiles at a time
        /// </summary>
        /// <param name="profiles">array profiles with a maximum of 50 profiles</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Bulk(Profile[] profiles)
        {
            if (profiles.Length > 50)
                throw new Exception("Maximum of 50 profiles allowed at a time");

            JArray arr = new JArray();

            foreach (Profile c in profiles)
                arr.Add(c.ToJObject());

            return await client.PostAsync(url + "/bulk", new JObject { ["items"] = arr });
        }

        /// <summary>
        /// Updates a maximum of 50 profiles at a time
        /// </summary>
        /// <param name="profiles">JArray containing profiles with a maximum of 50 profiles</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Bulk(JArray profiles)
        {
            if (profiles.Count > 50)
                throw new Exception("Maximum of 50 profiles allowed at a time");

            return await client.PostAsync(url + "/bulk", new JObject { ["items"] = profiles });
        }
    }
}
