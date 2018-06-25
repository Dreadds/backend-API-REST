using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class InventoryEntities
    {
        public Int32? inventoryId { set; get; }
        public Int32 quantity { set; get; }
    }
}