using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Datatrics.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Datatrics.Module
{
    /// <summary>
    /// Sales module, used for sales api calls
    /// </summary>
    public class SaleModule : BaseModule
    {
        /// <summary>
        /// BaseModule url for this module
        /// </summary>
        public override string url => "/project/" + client.ProjectId + "/sale";

        internal SaleModule(Client client) : base(client)
        {
        }

        /// <summary>
        /// Gets a sale by id
        /// </summary>
        /// <param name="saleId">Sale id</param>
        /// <returns>Result of the request</returns>
        public async Task<Sale> Get(string saleId)
        {
            JObject response = await client.GetAsync(url + "/" + saleId, new Dictionary<string, object>());

            JToken token;
            if (!response.TryGetValue("conversionid", out token))
                throw new Exception(response.ToString(Formatting.None));

            return new Sale(response);
        }

        /// <summary>
        /// Gets a list of sale objecten
        /// </summary>
        /// <param name="args">Query arguments, limit is set to a default of 25</param>
        /// <returns>Result of the request</returns>
        public async Task<ListResult<Sale>> GetList(Dictionary<string, object> args = null)
        {
            if (args == null)
                args = new Dictionary<string, object>();

            if (!args.ContainsKey("limit"))
                args.Add("limit", "25");

            JObject response = await client.GetAsync(url, args);

            JToken token;
            if (!response.TryGetValue("total_elements", out token))
                throw new Exception(response.ToString(Formatting.None));

            return new ListResult<Sale>(response);
        }

        /// <summary>
        /// Creates new sale
        /// </summary>
        /// <param name="sale">Sale object</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Create(Sale sale)
        {
            return await Create(sale.ToJObject());
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

        /// <summary>
        /// Updates a maximum of 50 sales at a time
        /// </summary>
        /// <param name="sales">array sales with a maximum of 50 sales</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Bulk(Sale[] sales)
        {
            if (sales.Length > 50)
                throw new Exception("Maximum of 50 sales allowed at a time");

            JArray arr = new JArray();

            foreach (Sale c in sales)
                arr.Add(c.ToJObject());

            return await client.PostAsync(url + "/bulk", new JObject { ["items"] = arr });
        }

        /// <summary>
        /// Updates a maximum of 50 sales at a time
        /// </summary>
        /// <param name="sales">JArray containing sales with a maximum of 50 sales</param>
        /// <returns>Result of the request</returns>
        public async Task<JObject> Bulk(JArray sales)
        {
            if (sales.Count > 50)
                throw new Exception("Maximum of 50 sales allowed at a time");

            return await client.PostAsync(url + "/bulk", new JObject { ["items"] = sales });
        }
    }
}
