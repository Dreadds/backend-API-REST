using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class SaleDetailEntities
    {
        public Int32? saleDetailId { set; get; }
        public Int32 saleId { set; get; }
        public Int32 productId { set; get; }
        public float priceSubTotal { set; get; }
        public Int32 quantity { set; get; }
        public String state { set; get; }
        public float unitPrice { set; get; }
    }
}