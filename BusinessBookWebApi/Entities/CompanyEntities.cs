using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class CompanyEntities
    {
        public Int32? companyId { set; get; }
        public String name { set; get; }
        public String state { set; get; }
        public Int32 locationId { set; get; }
        public Int32 employeeId { set; get; }
    }
}