﻿using BusinessBookWebApi.Helpers;
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
    public class LocalsController : BaseApiController
    {
        
        [Route("companies/{companyId}/locals")]
        [Route("companies/{companyId}/locals/{localId}")]
        [HttpGet]
        public HttpResponseMessage ListLocals(Int32? companyId = null, Int32 ? localId=null)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                var employeeId = GetEmployeeId();

                if (!employeeId.HasValue)
                {

                    Httpresponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Code = HttpStatusCode.Unauthorized;
                    response.Message = "Authorization has been denied for this request.";
                    response.Result = null;

                    Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return Httpresponse;
                }
                else
                {

                    if (companyId.HasValue)
                    {

                        Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                        response.Code = HttpStatusCode.OK;
                        response.Message = "success";
                        if (localId.HasValue)
                        {

                            response.Result = context.Local.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.LocalId == localId && x.CompanyId == companyId)
                                .Select(x => new
                                {
                                    localId = x.LocalId,
                                    name = x.Name,
                                    direction = x.Direction,
                                    company = x.Company.Name,
                                    state = x.State
                                }).ToList();

                        }
                        else
                        {

                            response.Result = context.Local.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.CompanyId == companyId)
                                .Select(x => new
                                {
                                    localId = x.LocalId,
                                    name = x.Name,
                                    direction = x.Direction,
                                    company = x.Company.Name,
                                    state = x.State
                                }).ToList();

                        }
                    }
                    else
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                        return Httpresponse;
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
       
        [Route("locals")]
        [HttpPost]
        public HttpResponseMessage AddLocal(LocalEntities model)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                var id = GetEmployeeId();

                if (!id.HasValue)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Code = HttpStatusCode.Unauthorized;
                    response.Message = "Authorization has been denied for this request.";
                    response.Result = null;

                    Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
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
                        var local = new Local();
                        context.Local.Add(local);
                        
                        local.Name = model.name;
                        local.Direction = model.direction;
                        local.CompanyId = model.companyId;
                        local.State = ConstantHelper.Status.ACTIVE;
                        context.SaveChanges();
                    }
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Code = HttpStatusCode.OK;
                    response.Message = "Success";
                    response.Result = null;
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

        
        [Route("locals/{localId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteLocal(Int16? LocalId=null)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                var id = GetEmployeeId();

                if (!id.HasValue)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Code = HttpStatusCode.Unauthorized;
                    response.Message = "Authorization has been denied for this request.";
                    response.Result = null;

                    Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return Httpresponse;
                }
                else
                {
                    if (LocalId == null)
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                        return Httpresponse;
                    }
                    else
                    {
                        var local = new Local();
                        local = context.Local.FirstOrDefault(x => x.LocalId == LocalId);
                        local.State = ConstantHelper.Status.INACTIVE;
                        context.SaveChanges();
                    }

                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Code = HttpStatusCode.OK;
                    response.Message = "Success";
                    response.Result = null;
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

        
        [Route("locals")]
        [HttpPut]
        public HttpResponseMessage UpdateLocal(LocalEntities model)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                var id = GetEmployeeId();

                if (!id.HasValue)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Code = HttpStatusCode.Unauthorized;
                    response.Message = "Authorization has been denied for this request.";
                    response.Result = null;

                    Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
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

                        var local = context.Local.FirstOrDefault(x => x.State == ConstantHelper.Status.ACTIVE && x.LocalId == model.localId);
                        
                        local.Name = model.name;
                        local.Direction = model.direction;
                        local.CompanyId = model.companyId;

                        context.SaveChanges();
                    }

                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Code = HttpStatusCode.OK;
                    response.Message = "Success";
                    response.Result = null;
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
