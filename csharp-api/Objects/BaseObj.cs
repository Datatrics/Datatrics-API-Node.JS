using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Datatrics.Objects
{
    public abstract class BaseObj
    {
        internal BaseObj()
        {
            
        }
        public abstract JObject ToJObject();
        public abstract void Deserialize(JObject obj);
    }
}
