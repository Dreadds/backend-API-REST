using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessBookWebApi.Models;

namespace BusinessBookWebApi.Entities
{
    public class Cliente
    {
        public String Status { set; get; }
        public List<Client> LstClients { set; get; } 
        /*public String Name { set; get; }
        public String LastName { set; get; }
        public String FullName { set; get; }
        public String DNI { set; get; }
        public String Email { set; get; }
        public String Phone { set; get; }
        public String LocationName { set; get; }
        public DateTime DateCreation { set; get; }
        public DateTime DateUpdate { set; get; }
        public String State { set; get; }
        public String Sexo { set; get; }*/
    }
}