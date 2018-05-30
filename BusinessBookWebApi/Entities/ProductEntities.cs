using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class ProductEntities
    {
        public Int32 productId { set; get; }
        public String name { set; get; }
        public float unitPrice { set; get; }
        //public String state { set; get; }
    }
}