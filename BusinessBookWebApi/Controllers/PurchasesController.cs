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
        [Route("purchases")]
        [Route("purchases/{purchaseId}")]
        [HttpGet]
        public HttpResponseMessage ListPurchases(Int32? PurchaseId)
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
                    return Httpresponse;
                }
                else
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Code = HttpStatusCode.OK;
                    response.Message = "success";
                    if (PurchaseId.HasValue)
                    {
                        response.Result = context.Purchase.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.PurchaseId == PurchaseId)
                            .Select(x => new
                            {
                                PurchaseId = x.PurchaseId,
                                DateCreation = x.DateCreation,
                                CodeGuide = x.CodeGuide,
                                LocalName = x.Local.Name,
                                PriceTotal = x.PriceTotal,
                                ProveedorName = x.Provider.Name,
                                State = x.State
                            }).ToList();

                    }
                    else
                    {
                        response.Result = context.Purchase.Where(x => x.State == ConstantHelper.Status.ACTIVE)
                            .Select(x => new
                            {
                                PurchaseId = x.PurchaseId,
                                DateCreation = x.DateCreation,
                                CodeGuide = x.CodeGuide,
                                LocalName = x.Local.Name,
                                PriceTotal = x.PriceTotal,
                                ProveedorName = x.Provider.Name,
                                State = x.State
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
        [Route("purchases")]
        [HttpPost]
        public HttpResponseMessage AddPurchase(PurchaseEntities model)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                if (model == null)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                    return Httpresponse;
                }
                else
                {
                    var purchase = new Purchase();
                    context.Purchase.Add(purchase);

                    purchase.PurchaseId = model.purchaseId;
                    purchase.DateCreation = DateTime.Today;
                    purchase.CodeGuide = model.codeGuide;
                    purchase.LocalId = model.localId;
                    purchase.State = ConstantHelper.Status.ACTIVE;
                    purchase.PriceTotal = model.priceTotal;

                    context.SaveChanges();
                }
                Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                return Httpresponse;
            }
            catch
            {
                Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
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
                if (model == null || PurchaseId == null)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                    return Httpresponse;
                }
                else
                {
                    var purchase = context.Purchase.FirstOrDefault(x => x.PurchaseId == PurchaseId);
                    foreach (var pD in model.listPurchaseDetail)
                    {

                        var purchaseDetail = new PurchaseDetail();
                        context.PurchaseDetail.Add(purchaseDetail);

                        purchaseDetail.PurchaseId = PurchaseId;
                        purchaseDetail.ProductId = pD.Item1;
                        purchaseDetail.Quantity = pD.Item2;
                        purchaseDetail.UnitPrice = pD.Item3;
                        purchaseDetail.PriceSubTotal = pD.Item4;
                        purchaseDetail.State = ConstantHelper.Status.ACTIVE;
                        context.SaveChanges();
                    }
                }
                Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                return Httpresponse;
            }
            catch
            {
                Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                return Httpresponse;
            }
        }

        [Authorize]
        [Route("purchases/{purchaseId}")]
        [HttpDelete]
        public HttpResponseMessage DeletePurchase(Int16 PurchaseId)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                if (PurchaseId == null)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
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
                return Httpresponse;
            }
            catch
            {
                Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
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

                if (model == null)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                    return Httpresponse;
                }
                else
                {

                    var purchase = context.Purchase.FirstOrDefault(x => x.State == ConstantHelper.Status.ACTIVE && x.PurchaseId == model.purchaseId);

                    purchase.PurchaseId = model.purchaseId;
                    purchase.CodeGuide = model.codeGuide;
                    purchase.LocalId = model.localId;
                    purchase.PriceTotal = model.priceTotal;

                    context.SaveChanges();
                }

                Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
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
