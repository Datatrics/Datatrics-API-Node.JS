using Newtonsoft.Json.Linq;

namespace Datatrics.Objects
{
    public class Content : BaseObj
    {
        public string Id { get; set; }
        public string ItemType { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public JObject Attributes { get; set; }

        public Content() { }

        public Content(string id, string itemtype, string type, string source)
        {
            Id = id;
            ItemType = itemtype;
            Type = type;
            Source = source;
        }

        public Content(JObject obj)
        {
            Deserialize(obj);
        }

        public override JObject ToJObject()
        {
            JToken metadata;
            JObject item = new JObject();
            if (Attributes.TryGetValue("metadata", out metadata))
            {
                item.Merge(Attributes);
                item.Merge(metadata);
                item.Remove("metadata");
            }
            else
                item = Attributes;

            return new JObject
            {
                ["itemid"] = Id,
                ["itemtype"] = ItemType,
                ["type"] = Type,
                ["source"] = Source,
                ["item"] = item
            };
        }

        public override void Deserialize(JObject obj)
        {
            JObject metadata = (JObject)obj.GetValue("metadata");

            Id = (string)obj["id"];
            ItemType = (string)metadata["itemtype"];
            Type = (string)obj["type"];
            Source = (string)metadata["source"];

            obj.Remove("id");
            obj.Remove("type");
            metadata.Remove("itemtype");
            metadata.Remove("source");

            Attributes = obj;
        }
    }
}
