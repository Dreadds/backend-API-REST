using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class ProviderEntities
    {
        public Int32? providerId { set; get; }
        public String name { set; get; }
        public String state { set; get; }
        public String phone { set; get; }
        public String email { set; get; }
    }
}