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
using BusinessBookWebApi.Entities;
using System.Net.Http.Formatting;

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
            TokenEmployee tokenEmployee = new TokenEmployee();
            if (!ValidateToken(tokenString))
            {
                return null;
            }
            else
            {
                tokenEmployee = context.TokenEmployee.FirstOrDefault(x => x.AccessToken == tokenString);
            }
            var employeeId = context.Employee.FirstOrDefault(x => x.State == ConstantHelper.Status.ACTIVE && x.TokenEmployeeId == tokenEmployee.TokenEmployeeId); ;
            return employeeId.EmployeeId;
        }

        protected bool ValidateToken(String token)
        {
            try
            {
                var tokenEmployee = context.TokenEmployee.FirstOrDefault(x => x.AccessToken == token);
                //logica expirado o no 
                var currentHour = DateTime.Now.AddHours(-7);
                var expireIn = tokenEmployee.Expires.Value;

                bool bandera = true;
                if (currentHour >= expireIn) {
                    bandera = false;
                }
                else
                {
                    bandera = true;
                }
                return bandera;
            }
            catch
            {
                return false;
            }
        }

        protected String GeneretaToken(String users,String password)
        {
            var fecha = DateTime.Now.AddHours(-7);
            String baseAddress = "http://chemita96-001-site1.dtempurl.com";
            //String baseAddress = "http://localhost:16669";
            TokenEntities tokenEntities = new TokenEntities();
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                                {
                                    {"grant_type", "password"},
                                    {"username", users},
                                    {"password", password},
                                };
                var tokenResponse = client.PostAsync(baseAddress + "/oauth/token", new FormUrlEncodedContent(form)).Result;
                //CONVERT 
                tokenEntities = tokenResponse.Content.ReadAsAsync<TokenEntities>(new[] { new JsonMediaTypeFormatter() }).Result;
                if (tokenEntities.accessToken == null)
                {
                    return null;
                }
                else
                {
                    var EmployeeBD = new Employee();
                    EmployeeBD = context.Employee.First(x => x.Users == users);

                    EmployeeBD.TokenEmployee.AccessToken = tokenEntities.accessToken;
                    EmployeeBD.TokenEmployee.ExpireInToken = tokenEntities.expiresIn;
                    EmployeeBD.TokenEmployee.ErrorToken = tokenEntities.error;
                    EmployeeBD.TokenEmployee.TypeToken = tokenEntities.tokenType;
                    EmployeeBD.TokenEmployee.RefreshToken = tokenEntities.refreshToken;
                    EmployeeBD.TokenEmployee.Issued = fecha;
                    EmployeeBD.TokenEmployee.Expires = fecha.AddHours(24);
                    EmployeeBD.TokenEmployee.State = ConstantHelper.Status.ACTIVE;

                    context.SaveChanges();
                }
                return tokenEntities.accessToken;
            }
        }
    }
}
