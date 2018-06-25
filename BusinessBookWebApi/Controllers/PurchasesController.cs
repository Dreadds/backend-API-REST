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
    public class PurchasesController : BaseApiController
    {
        [Authorize]
        [Route("companies/{companyId}/purchases")]
        [Route("companies/{companyId}/purchases/{purchaseId}")]
        [HttpGet]
        public HttpResponseMessage ListPurchases(Int32? companyId = null, Int32? purchaseId=null)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                // VERIFY IF TOKEN IS ENABLED
                var employeeId = GetEmployeeId();
                if (!employeeId.HasValue)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    response.Code = HttpStatusCode.Unauthorized;
                    response.Message = "Unauthorized";
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
                        if (purchaseId.HasValue)
                        {
                            response.Result = context.Purchase.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.PurchaseId == purchaseId && x.CompanyId == companyId)
                                .Select(x => new
                                {
                                    purchaseId = x.PurchaseId,
                                    dateCreation = x.DateCreation,
                                    codeGuide = x.CodeGuide,
                                    localName = x.Local.Name,
                                    priceTotal = x.PriceTotal,
                                    proveedorName = x.Provider.Name,
                                    state = x.State
                                }).ToList();

                        }
                        else
                        {
                            response.Result = context.Purchase.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.CompanyId == companyId)
                                .Select(x => new
                                {
                                    purchaseId = x.PurchaseId,
                                    dateCreation = x.DateCreation,
                                    codeGuide = x.CodeGuide,
                                    localName = x.Local.Name,
                                    priceTotal = x.PriceTotal,
                                    proveedorName = x.Provider.Name,
                                    state = x.State
                                }).ToList();
                        }
                    }
                    else
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.Code = HttpStatusCode.NotFound;
                        response.Message = "Not Found";
                        response.Result = null;
                        Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                        Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
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
                response.Code = HttpStatusCode.BadGateway;
                response.Message = "Bad Gateway";
                response.Result = null;
                Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return Httpresponse;
            }
        }
        [Authorize]
        [Route("purchases")]
        [HttpPost]
        public HttpResponseMessage AddPurchase(PurchaseEntities model)
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
                    Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return Httpresponse;
                }
                else
                {
                    var purchase = new Purchase();
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
                       
                        context.Purchase.Add(purchase);
                        
                        purchase.DateCreation = DateTime.Today;
                        purchase.CodeGuide = model.codeGuide;
                        purchase.LocalId = model.localId;
                        purchase.State = ConstantHelper.Status.ACTIVE;
                        purchase.PriceTotal = model.priceTotal;
                        purchase.ProviderId = model.providerId;
                        purchase.CompanyId = model.companyId;
                        context.SaveChanges();
                    }
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Code = HttpStatusCode.OK;
                    response.Message = "Success";
                    response.Result = purchase;
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

        [Authorize]
        [Route("purchases/{purchaseId}/items")]
        [HttpPost]
        public HttpResponseMessage AddPurchaseDetail(Int32 PurchaseId, PurchaseDetailEntities model)
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
                    Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return Httpresponse;
                }
                else
                {
                    if (model == null || PurchaseId == null)
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
                        var purchase = context.Purchase.FirstOrDefault(x => x.PurchaseId == PurchaseId);
                        var total = 0.0;
                        foreach (var pD in model.listPurchaseDetail)
                        {

                            var purchaseDetail = new PurchaseDetail();
                            context.PurchaseDetail.Add(purchaseDetail);

                            purchaseDetail.PurchaseId = PurchaseId;
                            purchaseDetail.ProductId = pD.productId;
                            purchaseDetail.Quantity = pD.quantity;
                            purchaseDetail.UnitPrice = pD.unitPrice;
                            purchaseDetail.PriceSubTotal = pD.priceSubTotal;
                            purchaseDetail.State = ConstantHelper.Status.ACTIVE;
                            context.SaveChanges();

                            var inventory = new Inventory();

                            inventory = context.Inventory.FirstOrDefault(x => x.LocalId == purchase.LocalId
                            && x.ProductId == purchaseDetail.ProductId
                            && x.State == ConstantHelper.Status.ACTIVE);

                            inventory.Quantity = inventory.Quantity + purchaseDetail.Quantity;
                            inventory.DateUpdate = DateTime.Today;
                            total = total + pD.priceSubTotal;
                            context.SaveChanges();
                        }
                        purchase.PriceTotal = total;
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

        [Authorize]
        [Route("purchases/{purchaseId}")]
        [HttpDelete]
        public HttpResponseMessage DeletePurchase(Int32? PurchaseId = null)
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
                    Httpresponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    Httpresponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return Httpresponse;
                }
                else
                {
                    if (PurchaseId == null)
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
                        var purchase = new Purchase();
                        purchase = context.Purchase.FirstOrDefault(x => x.PurchaseId == PurchaseId);
                        purchase.State = ConstantHelper.Status.INACTIVE;
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

        [Authorize]
        [Route("purchases")]
        [HttpPut]
        public HttpResponseMessage UpdatePurchases(PurchaseEntities model)
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

                        var purchase = context.Purchase.FirstOrDefault(x => x.State == ConstantHelper.Status.ACTIVE && x.PurchaseId == model.purchaseId);
                        
                        purchase.CodeGuide = model.codeGuide;
                        purchase.LocalId = model.localId;
                        purchase.PriceTotal = model.priceTotal;
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
