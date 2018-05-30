using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class DetailEntities
    {
        public Int32 productId { set; get; }
        public Int32 quantity { set; get; }
        public float unitPrice { set; get; }
        public float priceSubTotal { set; get; }
    }
}