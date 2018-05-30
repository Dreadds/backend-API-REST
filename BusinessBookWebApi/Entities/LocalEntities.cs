using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class LocalEntities
    {
        public Int32 localId { set; get; }
        public String name { set; get; }
        //public String state { set; get; }
        public String direction { set; get; }
        public Int32 companyId { set; get; }
    }
}