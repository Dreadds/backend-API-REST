using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BusinessBookWebApi.Models;
using BusinessBookWebApi.Helpers;
using BusinessBookWebApi.Logics;
using System.Web.Routing;
using System.Security.Claims;

namespace BusinessBookWebApi.Controllers
{
    public class BaseApiController : ApiController
    {
        protected BusinessBookEntities context = new BusinessBookEntities();
        protected Response response = new Response();
        protected Token token = new Token();
        protected Int32? GetEmployeeId()
        {
            var token = Request.Headers.GetValues(ConstantHelper.TOKEN_HEADER_NAME).First();
            var test = Request.Headers.GetValues(ConstantHelper.TOKEN_HEADER_NAME);

            var array = token.Split(' ');
            var tokenString = array[1];

            if (!ValidateToken(tokenString))
            {
                return null;
            }

            var tokenEmployee = context.TokenEmployee.FirstOrDefault(x => x.AccessToken == tokenString);
            var employeeId = context.Employee.FirstOrDefault(x => x.State == ConstantHelper.Status.ACTIVE && x.TokenEmployeeId == tokenEmployee.TokenEmployeeId); ;
            
            return employeeId.EmployeeId;
        }

        protected bool ValidateToken(String token)
        {
            try
            {
                var tokenEmployee = context.TokenEmployee.FirstOrDefault(x => x.AccessToken == token);
                //logica expirado o no 
                var currentHour = DateTime.Now.TimeOfDay;
                var expireIn = tokenEmployee.Expires.Value.TimeOfDay;
                bool bandera = currentHour >= expireIn ? false : true;
                return bandera;
            }
            catch
            {
                return false;
            }
        }
    }
}
