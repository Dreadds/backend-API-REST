using BusinessBookWebApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using BusinessBookWebApi.Models;
using BusinessBookWebApi.Entities;

namespace BusinessBookWebApi.Controllers
{
    [RoutePrefix("businessbookapi/v1")]
    public class ProvidersController : BaseApiController
    {
        [Authorize]
        [Route("providers")]
        [Route("providers/{providerId}")]
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
        [Authorize]
        [Route("providers")]
        [HttpPost]
        public HttpResponseMessage AddProvider(ProviderEntities model)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                var id = GetEmployeeId();

                if (!id.HasValue)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Code = HttpStatusCode.Unauthorized;
                    response.Message = "Unauthorized";
                    response.Result = null;
                    return Httpresponse;
                }
                else
                {
                    if (model == null)
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                        return Httpresponse;
                    }
                    else
                    {
                        var provider = new Provider();
                        context.Provider.Add(provider);
                        
                        provider.Name = model.name;
                        provider.email = model.email;
                        provider.Phone = model.phone;
                        provider.State = ConstantHelper.Status.ACTIVE;
                        context.SaveChanges();
                    }
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                }
                return Httpresponse;
            }
            catch
            {
                Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                return Httpresponse;
            }
        }

        [Authorize]
        [Route("providers/{providerId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteProvider(Int16 ProviderId)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                var id = GetEmployeeId();

                if (!id.HasValue)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Code = HttpStatusCode.Unauthorized;
                    response.Message = "Unauthorized";
                    response.Result = null;
                    return Httpresponse;
                }
                else
                {
                    if (ProviderId == null)
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                        return Httpresponse;
                    }
                    else
                    {
                        var provider = new Provider();
                        provider = context.Provider.FirstOrDefault(x => x.ProviderId == ProviderId);
                        provider.State = ConstantHelper.Status.INACTIVE;
                        context.SaveChanges();
                    }

                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                }
                return Httpresponse;
            }
            catch
            {
                Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                return Httpresponse;
            }
        }

        [Authorize]
        [Route("providers")]
        [HttpPut]
        public HttpResponseMessage UpdateProvider(ProviderEntities model)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                var id = GetEmployeeId();

                if (!id.HasValue)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Code = HttpStatusCode.Unauthorized;
                    response.Message = "Unauthorized";
                    response.Result = null;
                    return Httpresponse;
                }
                else
                {
                    if (model == null)
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                        return Httpresponse;
                    }
                    else
                    {

                        var provider = context.Provider.FirstOrDefault(x => x.State == ConstantHelper.Status.ACTIVE && x.ProviderId == model.providerId);
                        
                        provider.Name = model.name;
                        provider.email = model.email;
                        provider.Phone = model.phone;
                        provider.State = ConstantHelper.Status.ACTIVE;

                        context.SaveChanges();
                    }

                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                }
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
