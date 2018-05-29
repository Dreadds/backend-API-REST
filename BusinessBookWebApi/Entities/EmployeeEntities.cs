using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class EmployeeEntities
    {
        public Int32? employeeId { set; get; }
        public String name { set; get; }
        public String lastName { set; get; }
        public String fullName { set; get; }
        public String dni { set; get; }
        public String email { set; get; }
        public String phone { set; get; }
        public Int32 locationId { set; get; }
        public String sex{ set; get; }
        public String users { set; get; }
        public String password { set; get; }
    }
}