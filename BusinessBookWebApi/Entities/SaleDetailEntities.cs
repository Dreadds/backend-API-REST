using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class SaleDetailEntities
    {
        //Product - Quantity - UnitPrice - PriceSubTotal
        public List<Tuple<Int32, Int32, float, float>> listProductSale { set; get; } = new List<Tuple<int?, int?, float, float>>();
    }
}