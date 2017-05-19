using Newtonsoft.Json.Linq;

namespace Datatrics.Objects
{
    public class Profile : BaseObj
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public JObject Attributes { get; set; }

        public Profile() { }

        public Profile(string id, string source)
        {
            Id = id;
            Source = source;
        }

        internal Profile(JObject obj)
        {
            Deserialize(obj);
        }

        public override JObject ToJObject()
        {
            return new JObject
            {
                ["profileid"] = Id,
                ["source"] = Source,
                ["profile"] = Attributes
            };
        }

        public override void Deserialize(JObject obj)
        {
            Id = (string)obj["profileid"];
            Source = (string)obj["source"];

            obj.Remove("profileid");
            obj.Remove("objecttype");
            obj.Remove("source");

            Attributes = obj;
        }
    }
}
