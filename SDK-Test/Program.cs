using System;
using Datatrics;
using Newtonsoft.Json.Linq;

namespace SDK_Test
{
    class Program
    {
        static void Main()
        {
            Test();

            Console.ReadLine();
        }

        static async void Test()
        {
            Client client = new Client("6105a95b2908e14c6a0a0627ec0db235", 255062);
            
            JObject resposne = await client.Content.Get("98");

            Console.WriteLine(resposne.ToString());
        }
    }
}
