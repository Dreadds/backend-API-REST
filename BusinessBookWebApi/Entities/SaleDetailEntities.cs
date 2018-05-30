using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class SaleDetailEntities
    {
        //Product - Quantity - UnitPrice - PriceSubTotal
        public List<DetailEntities> listSaleDetail { set; get; } = new List<DetailEntities>();
    }
    
}