using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class PurchaseDetailEntities
    {
        public Int32? purchaseDetailId { set; get; }
        public Int32? purchaseId { set; get; }
        public Int32? ProductId { set; get; }
        public float priceSubTotal { set; get; }
        public Int32? quantity { set; get; }
        public String state { set; get; }
        public float unitPrice { set; get; }
    }
}