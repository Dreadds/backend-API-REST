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
    public class SalesController : BaseApiController
    {
        [HttpGet]
        [Route("locals/{localId}/dates/{datetime}/saledetails")]
        public HttpResponseMessage ListSaleDetailProductLocalDate(Int32? localId = null, String datetime = null)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                var employeeId = GetEmployeeId();
                var date = DateTime.Parse(datetime);
                if (!employeeId.HasValue)
                {

                    Httpresponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Code = HttpStatusCode.Unauthorized;
                    response.Message = "Authorization has been denied for this request.";
                    response.Result = null;
                    return Httpresponse;
                }
                else
                {
                    if (localId.HasValue)
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                        response.Code = HttpStatusCode.OK;
                        response.Message = "success";

                        response.Result = context.SaleDetail.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.Sale.LocalId == localId && x.Sale.DateCreation == date)
                            .Select(x => new
                            {
                                saleDetailId = x.SaleDetailId,
                                productId = x.ProductId,
                                nameProduct = x.Product.Name,
                                dateCreation = x.Sale.DateCreation,
                                state = x.State
                            }).ToList();
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

        [HttpGet]
        [Route("companies/{companyId}/sales")]
        [Route("companies/{companyId}/sales/{saleId}")]
        public HttpResponseMessage ListSales(Int32? companyId = null, Int32? saleId = null)
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

                    Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response.Result = null;
                }
                else
                {
                    
                    if (companyId.HasValue)
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                        response.Code = HttpStatusCode.OK;
                        response.Message = "success";
                        if (saleId.HasValue)
                        {

                            response.Result = context.Sale.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.SaleId == saleId && x.CompanyId == companyId)
                                .Select(x => new
                                {
                                    saleId = x.SaleId,
                                    dateCreation = x.DateCreation,
                                    codeGuide = x.CodeGuide,
                                    localName = x.Local.Name,
                                    priceTotal = x.PriceTotal,
                                    employeeName = x.Employee.Name,
                                    clientName = x.Client.Name,
                                    state = x.State,
                                    stateDelivery = x.StateDelivery
                                }).ToList();

                        }
                        else
                        {

                            response.Result = context.Sale.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.CompanyId == companyId)
                                .Select(x => new
                                {
                                    saleId = x.SaleId,
                                    dateCreation = x.DateCreation,
                                    codeGuide = x.CodeGuide,
                                    localName = x.Local.Name,
                                    priceTotal = x.PriceTotal,
                                    employeeName = x.Employee.Name,
                                    clientName = x.Client.Name,
                                    state = x.State,
                                    stateDelivery = x.StateDelivery
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
            }
            catch
            {
                Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
            }
            return Httpresponse;
        }
        
        [Route("sales")]
        [HttpPost]
        public HttpResponseMessage AddSale(SaleEntities model)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                var id = GetEmployeeId();

                if (!id.HasValue)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Code = HttpStatusCode.Unauthorized;
                    response.Message = "Unautorized";
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
                        response.Code = HttpStatusCode.BadGateway;
                        response.Message = "Bad Gateway";
                        response.Result = null;
                        Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                        Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return Httpresponse;
                    }
                    else
                    {
                        var sale = new Sale();
                        context.Sale.Add(sale);
                        
                        sale.DateCreation = DateTime.Today;
                        sale.CodeGuide = model.codeGuide;
                        sale.LocalId = model.localId;
                        sale.ClientId = model.clientId;
                        sale.State = ConstantHelper.Status.ACTIVE;
                        sale.StateDelivery = ConstantHelper.Status.ACTIVE;
                        sale.CompanyId = model.companyId;
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
            catch (Exception ex)
            {
                Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                response.Code = HttpStatusCode.BadGateway;
                response.Message = "Bad Gateway";
                response.Result = null;
                Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return Httpresponse;
            }
        }
        
        [Route("sales/{saleId}/items")]
        [HttpPost]
        public HttpResponseMessage AddSaleDetail(Int32 SaleId, SaleDetailEntities model)
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
                    return Httpresponse;
                }
                else
                {
                    if (model == null || SaleId == null)
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                        response.Code = HttpStatusCode.BadGateway;
                        response.Message = "Bad Gateway";
                        response.Result = null;
                        Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                        Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return Httpresponse;
                    }
                    else
                    {
                        var sale = context.Sale.FirstOrDefault(x => x.SaleId == SaleId);
                        var total = 0.0;
                        foreach (var sD in model.listSaleDetail)
                        {

                            var saleDetail = new SaleDetail();
                            context.SaleDetail.Add(saleDetail);

                            saleDetail.SaleId = SaleId;
                            saleDetail.ProductId = sD.productId;
                            saleDetail.Quantity = sD.quantity;
                            saleDetail.UnitPrice = sD.unitPrice;
                            saleDetail.PriceSubTotal = sD.priceSubTotal;
                            saleDetail.State = ConstantHelper.Status.ACTIVE;
                            context.SaveChanges();

                            var inventory = new Inventory();

                            inventory = context.Inventory.FirstOrDefault(x => x.LocalId == sale.LocalId
                            && x.ProductId == saleDetail.ProductId
                            && x.State == ConstantHelper.Status.ACTIVE);

                            inventory.Quantity = inventory.Quantity - saleDetail.Quantity;
                            inventory.DateUpdate = DateTime.Today;
                            total = total + sD.priceSubTotal;
                            context.SaveChanges();
                        }
                        sale.PriceTotal = total;
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
                response.Code = HttpStatusCode.BadGateway;
                response.Message = "Bad Gateway";
                response.Result = null;
                Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return Httpresponse;
            }
        }
        
        [Route("sales/{saleId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteSale(Int32? SaleId=null)
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
                    if (SaleId == null)
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                        response.Code = HttpStatusCode.BadGateway;
                        response.Message = "Bad Gateway";
                        response.Result = null;
                        Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                        Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return Httpresponse;
                    }
                    else
                    {
                        var sale = new Sale();
                        sale = context.Sale.FirstOrDefault(x => x.SaleId == SaleId);
                        sale.State = ConstantHelper.Status.INACTIVE;
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
                response.Code = HttpStatusCode.BadGateway;
                response.Message = "Bad Gateway";
                response.Result = null;
                Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return Httpresponse;
            }
        }
        
        [Route("sales")]
        [HttpPut]
        public HttpResponseMessage UpdateSale(SaleEntities model)
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
                        response.Code = HttpStatusCode.BadGateway;
                        response.Message = "Bad Gateway";
                        response.Result = null;
                        Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                        Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        return Httpresponse;
                    }
                    else
                    {

                        var sale = context.Sale.FirstOrDefault(x => x.State == ConstantHelper.Status.ACTIVE && x.SaleId == model.saleId);
                        
                        sale.CodeGuide = model.codeGuide;
                        sale.LocalId = model.localId;
                        sale.ClientId = model.clientId;
                        sale.PriceTotal = model.priceTotal;
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
                response.Code = HttpStatusCode.BadGateway;
                response.Message = "Bad Gateway";
                response.Result = null;
                Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return Httpresponse;
            }
        }
    }
}
