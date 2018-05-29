using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class SaleEntities
    {
        public Int32 saleId { set; get; }
        public DateTime dateCreation { set; get; }
        public String codeGuide { set; get; }
        public Int32 localId { set; get; }
        public float priceTotal { set; get; }
        public Int32 EmployeeId { set; get; }
        public Int32 clientId { set; get; }
        public String state { set; get; }
        public String stateDelivery { set; get; }
        
    }
}