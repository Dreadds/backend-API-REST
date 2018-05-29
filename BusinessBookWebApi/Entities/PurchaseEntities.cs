using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class PurchaseEntities
    {
        public Int32 purchaseId { set; get; }
        public String codeGuide { set; get; }
        public Int32 providerId{ set; get; }
        public float priceTotal { set; get; }
        public DateTime dateCreation { set; get; }
        public String state { set; get; }
        public Int32 localId { set; get; }
    }
}