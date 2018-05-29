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
            Int32 dato = 0;
            return dato;
        }
    }
}
