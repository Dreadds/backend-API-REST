using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessBookWebApi.Models;
using BusinessBookWebApi.Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using BusinessBookWebApi.Logics;
using BusinessBookWebApi.Helpers;

namespace BusinessBookWebApi.Controllers
{
    [RoutePrefix("clients")]
    public class ClientsController : BaseApiController
    {
        
        [HttpGet]
        [Route("{clientId}")]
        public HttpResponseMessage ListClients(Int32? ClientId)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                var employeeId = GetEmployeeId();

                if (!employeeId.HasValue)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Code = HttpStatusCode.Unauthorized;
                    response.Message = "Unauthorized";
                    response.Result = null;
                }
                else
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Code = HttpStatusCode.OK;
                    response.Message = "success";

                    if (ClientId.HasValue)
                    { 
                        response.Result = context.Client.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.ClientId == ClientId)
                        .Select(x => new
                        {
                            Name = x.Name,
                            LastName = x.LastName,
                            FullName = x.FullName,
                            DNI = x.DNI,
                            Email = x.Email,
                            Phone = x.Phone,
                            Location = x.Location.Name,
                            DateCreation = x.DateCreation,
                            DateUpdate = x.DateUpdate,
                            State = x.State,
                            Sex = x.Sex == "M" ? "Male" : "Female"
                        }).ToList();

                    }
                    else
                    {

                        response.Result = context.Client.Where(x => x.State == ConstantHelper.Status.ACTIVE)
                        .Select(x => new
                        {
                            Name = x.Name,
                            LastName = x.LastName,
                            FullName = x.FullName,
                            DNI = x.DNI,
                            Email = x.Email,
                            Phone = x.Phone,
                            Location = x.Location.Name,
                            DateCreation = x.DateCreation,
                            DateUpdate = x.DateUpdate,
                            State = x.State,
                            Sex = x.Sex == "M" ? "Male" : "Female"
                        }).ToList();

                    }

                    Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
            }
            catch
            {
                Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
            }
            return Httpresponse;
        }
    }
}
