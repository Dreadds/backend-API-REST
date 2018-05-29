using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class PurchaseDetailEntities
    {
        //Product - Quantity - UnitPrice - PriceSubTotal
        public List<Tuple<Int32, Int32, float, float>> listPurchaseDetail { set; get; } = new List<Tuple<int, int, float, float>>();
    }
}