using BusinessBookWebApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace BusinessBookWebApi.Controllers
{
    [RoutePrefix("businessbookapi/v1")]
    public class ProvidersController : BaseApiController
    {
        [Authorize]
        [Route("provider")]
        [Route("provider/{providerId}")]
        [HttpGet]
        public HttpResponseMessage ListProviders(Int32? ProviderId)
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
                    return Httpresponse;
                }
                else
                {

                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Code = HttpStatusCode.OK;
                    response.Message = "success";

                    if (ProviderId.HasValue)
                    {

                        response.Result = context.Provider.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.ProviderId == ProviderId)
                            .Select(x => new
                            {
                                ProviderId = x.ProviderId,
                                Name = x.Name,
                                State = x.State,
                                Phone = x.Phone,
                                Email = x.email
                            }).ToList();

                    }
                    else
                    {

                        response.Result = context.Provider.Where(x => x.State == ConstantHelper.Status.ACTIVE)
                            .Select(x => new
                            {
                                ProviderId = x.ProviderId,
                                Name = x.Name,
                                State = x.State,
                                Phone = x.Phone,
                                Email = x.email
                            }).ToList();

                    }
                }

                Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return Httpresponse;
            }
            catch
            {
                Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                return Httpresponse;
            }
        }
    }
}
