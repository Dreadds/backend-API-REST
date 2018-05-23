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

namespace BusinessBookWebApi.Controllers
{
    public class BaseApiController : ApiController
    {
        protected BusinessBookEntities context = new BusinessBookEntities();
        protected Response response = new Response();

        protected Int32? GetEmployeeId()
        {
            try
            {
                var token = Request.Headers.GetValues(ConstantHelper.TOKEN_HEADER_NAME).First();
                var test = Request.Headers.GetValues(ConstantHelper.TOKEN_HEADER_NAME);

                if (!TokenLogic.ValidateToken(token, ConstantHelper.TOKEN_TIMEOUT)) ;

                var employee = context.Employee.FirstOrDefault(x => x.Token == token && x.State == ConstantHelper.Status.ACTIVE);

                return employee?.EmployeeId;
            }
            catch
            {
                return null;
            }
        }
    }
}
