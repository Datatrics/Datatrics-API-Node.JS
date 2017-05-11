using System;
using System.Collections.Generic;
using System.Text;

namespace Datatrics.Module
{
    public class Base
    {
        internal Client client;
        public virtual string url => "";

        internal Base(Client client)
        {
            this.client = client;
        }
    }
}
