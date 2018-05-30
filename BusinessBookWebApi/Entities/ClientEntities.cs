using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class ClientEntities
    {
        public Int32 clientId { set; get; }
        public String name { set; get; }
        public String lastName { set; get; }
        public String fullName { set; get; }
        public String dni { set; get; }
        public String email { set; get; }
        public String phone { set; get; }
        public Int32 locationId { set; get; }
        /*public DateTime dateCreation { set; get; }
        public DateTime dateUpdate { set; get; }
        public String state { set; get; }*/
        public String sex { set; get; }
    }
}