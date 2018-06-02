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
    [RoutePrefix("businessbookapi/v1")]
    public class ClientsController : BaseApiController
    {
        [Authorize]
        [Route("clients")]
        [Route("clients/{clientId}")]
        [HttpGet]
        public HttpResponseMessage ListClients(Int32? clientId = null)
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
                    response.Message = "Success";


                    if (clientId.HasValue)
                    {
                        response.Result = context.Client.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.ClientId == clientId)
                        .Select(x => new
                        {
                            name = x.Name,
                            lastName = x.LastName,
                            fullName = x.FullName,
                            dni = x.DNI,
                            email = x.Email,
                            phone = x.Phone,
                            location = x.Location.Name,
                            dateCreation = x.DateCreation,
                            dateUpdate = x.DateUpdate,
                            state = x.State,
                            sex = x.Sex == "M" ? "Male" : "Female"
                        }).ToList();

                    }
                    else
                    {

                        response.Result = context.Client.Where(x => x.State == ConstantHelper.Status.ACTIVE)
                            .Select(x => new
                            {
                                name = x.Name,
                                lastName = x.LastName,
                                fullName = x.FullName,
                                dni = x.DNI,
                                email = x.Email,
                                phone = x.Phone,
                                location = x.Location.Name,
                                dateCreation = x.DateCreation,
                                dateUpdate = x.DateUpdate,
                                state = x.State,
                                sex = x.Sex == "M" ? "Male" : "Female"
                            }).ToList();

                    }

                    Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return Httpresponse;
                }
            }
            catch
            {
                Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                return Httpresponse;
            }
        }

        [Authorize]
        [Route("clients")]
        [HttpPost]
        public HttpResponseMessage AddClient(ClientEntities model)
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

                        var client = new Client();
                        context.Client.Add(client);

                        client.ClientId = model.clientId;
                        client.Name = model.name;
                        client.LastName = model.lastName;
                        client.FullName = model.fullName;
                        client.DNI = model.dni;
                        client.Email = model.email;
                        client.Phone = model.phone;
                        client.LocationId = model.locationId;
                        client.DateCreation = DateTime.Today;
                        client.DateUpdate = DateTime.Today;
                        client.State = ConstantHelper.Status.ACTIVE;
                        client.Sex = model.sex;
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
        [Route("clients/{clientId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteCliente(Int16? Clientid = null)
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
                    if (Clientid == null)
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                        return Httpresponse;
                    }
                    else
                    {
                        var client = new Client();
                        client = context.Client.FirstOrDefault(x => x.ClientId == Clientid);
                        client.State = ConstantHelper.Status.INACTIVE;
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
        [Route("clients")]
        [HttpPut]
        public HttpResponseMessage UpdateCliente(ClientEntities model)
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

                        var client = context.Client.FirstOrDefault(x => x.State == ConstantHelper.Status.ACTIVE && x.ClientId == model.clientId);

                        client.ClientId = model.clientId;
                        client.Name = model.name;
                        client.LastName = model.lastName;
                        client.FullName = model.fullName;
                        client.DNI = model.dni;
                        client.Email = model.email;
                        client.Phone = model.phone;
                        client.LocationId = model.locationId;
                        client.DateUpdate = DateTime.Today;
                        client.State = ConstantHelper.Status.ACTIVE;
                        client.Sex = model.sex;

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
