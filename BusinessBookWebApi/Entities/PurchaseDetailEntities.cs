using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class PurchaseDetailEntities
    {
        //Product - Quantity - UnitPrice - PriceSubTotal
        public List<DetailEntities> listPurchaseDetail { set; get; } = new List<DetailEntities>();
    }
}