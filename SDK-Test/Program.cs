using System;
using System.Collections.Generic;
using Datatrics;
using Newtonsoft.Json.Linq;

namespace SDK_Test
{
    class Program
    {
        static void Main()
        {
            Client client = new Client("api-key", 1234);

            ProfileTest(client);
            ContentTest(client);
            SalesTest(client);

            Console.ReadLine();
        }

        static async void ContentTest(Client client)
        {
            
            // Create content
            JObject contentItem = new JObject
            {
                ["projectid"] = client.ProjectId,
                ["itemid"] = "content-0",
                ["itemtype"] = "product",
                ["type"] = "item",
                ["source"] = "MyWebshop",
                ["item"] = new JObject
                {
                    ["name"] = "Item 1",
                    ["description"] = "I am a description",
                    ["content"] = "Example of content",
                    ["url"] = "www.mywebshop.com/product/content-0",
                    ["image"] = "www.mywebshop.com/images/content-0",
                    ["price"] = "24,99",
                    ["stock"] = "12"
                }
            };
            JObject response = await client.Content.Create(contentItem);
            Console.WriteLine(response.ToString());

            // Get Content by id
            response = await client.Content.Get("content-0", new Dictionary<string, object>() { {"source", "MyWebShop" } });
            Console.WriteLine(response.ToString());
        }

        static async void ProfileTest(Client client)
        {
            JObject profile = new JObject
            {
                ["projectid"] = client.ProjectId,
                ["profileid"] = "profile-0",
                ["source"] = "MyWebShop",
                ["profile"] = new JObject
                {
                    ["profileid"] = "profile-0",
                    ["created"] = "2017-05-10 22:30:01",
                    ["updated"] = "2017-05-10 22:30:01",
                    ["company"] = "MyCompany",
                    ["dateofbirth"] = "1980-01-01",
                    ["firstname"] = "David",
                    ["lastname"] = "Brown",
                    ["zip"] = "8234KJ",
                    ["city"] = "Oldenzaal",
                    ["country"] = "The Netherlands",
                    ["address"] = "Vogelstraat 42",
                    ["phone"] = "0383381249",
                    ["nationality"] = "Dutch",
                    ["mobile"] = "062415629",
                    ["email"] = "davidbrown@example.com",
                    ["lang"] = "NL",
                    ["gender"] = "Male",
                }
            };

            JObject response = await client.Profile.Create(profile);
            Console.WriteLine(response);

            response = await client.Profile.Get("profile-0", new Dictionary<string, object>() { { "source", "MyWebShop" } });
            Console.WriteLine(response);
        }

        static async void SalesTest(Client client)
        {
            JObject sale = new JObject
            {
                ["projectid"] = client.ProjectId,
                ["conversionid"] = "conversion-0",
                ["objecttype"] = "conversion",
                ["source"] = "MyWebShop",
                ["conversion"] = new JObject
                {
                    ["profileid"] = "profile-0",
                    ["total"] = 49.98,
                    ["status"] = "payment confirmed",
                    ["items"] = new JArray
                    {
                        new JObject
                        {
                            ["itemid"] = "content-0",
                            ["sku"] = "sku-content-0",
                            ["name"] = "Item 1",
                            ["quantity"] = 2,
                            ["price"] = 24.99,
                            ["total"] = 48.98
                        }
                    }
                }
            };

            JObject response = await client.Sale.Create(sale);
            Console.WriteLine(response);

            response = await client.Sale.Get("conversion-0");
            Console.WriteLine(response);
        }
    }
}
