using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Datatrics.Module
{
    public class Sale : Base
    {
        public override string url => "/project/" + client.ProjectId + "/sale";

        internal Sale(Client client) : base(client)
        {
        }

        /// <summary>
        /// Gets a list of sales or a specific one
        /// </summary>
        /// <param name="saleId">Sale id</param>
        /// <param name="args">Query arguments, limit is set to a default of 25</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Get(string saleId = null, Dictionary<string, object> args = null)
        {
            if (args == null)
                args = new Dictionary<string, object> { { "limit", "25" } };

            if (!String.IsNullOrEmpty(saleId))
                return await client.GetAsync(url + "/" + saleId, args);

            return await client.GetAsync(url, args);
        }

        /// <summary>
        /// Creates new sale
        /// </summary>
        /// <param name="sale">Sale object</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Create(JObject sale)
        {
            return await client.PostAsync(url, sale);
        }

        /// <summary>
        /// Deletes sale by id
        /// </summary>
        /// <param name="saleid">Sale id</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Delete(string saleid)
        {
            return await client.DeleteAsync(url + "/" + saleid);
        }
    }
}
