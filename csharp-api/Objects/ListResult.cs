using Newtonsoft.Json.Linq;

namespace Datatrics.Objects
{
    public class ListResult<T> where T : BaseObj, new()
    {
        public int TotalElements { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int Size { get; set; }
        public T[] Items { get; set; }

        internal ListResult(JObject obj)
        {
            TotalElements = (int) obj["total_elements"];
            TotalPages = (int) obj["total_pages"];
            CurrentPage = (int) obj["page"];
            Size = (int) obj["size"];

            JArray arr = (JArray)obj["items"];
            Items = new T[arr.Count];

            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new T();
                Items[i].Deserialize((JObject)arr[i]);
            }

        }
    }
}
