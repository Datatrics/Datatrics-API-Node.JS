using Newtonsoft.Json.Linq;

namespace Datatrics.Objects
{
    public class Sale : BaseObj
    {
        public string Id { get; set; }
        public string ObjectType { get; set; }
        public string Source { get; set; }
        public JObject Attributes { get; set; }

        public Sale() { }

        public Sale(string id, string objecttype, string source)
        {
            Id = id;
            ObjectType = objecttype;
            Source = source;
        }

        public Sale(JObject obj)
        {
            Deserialize(obj);
        }

        public override JObject ToJObject()
        {
            return new JObject
            {
                ["conversionid"] = Id,
                ["objecttype"] = ObjectType,
                ["source"] = Source,
                ["conversion"] = Attributes
            };
        }

        public override void Deserialize(JObject obj)
        {
            Id = (string)obj["conversionid"];
            ObjectType = (string)obj["objecttype"];
            Source = (string)obj["source"];

            obj.Remove("conversionid");
            obj.Remove("objecttype");
            obj.Remove("source");

            Attributes = obj;
        }
    }
}
