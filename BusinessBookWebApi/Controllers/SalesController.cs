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
        [Authorize]
        [HttpGet]
        [Route("sales")]
        [Route("sales/{saleId}")]
        public HttpResponseMessage ListSales(Int32? SaleId)
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
                    if (SaleId.HasValue)
                    {

                        response.Result = context.Sale.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.SaleId == SaleId)
                            .Select(x => new
                            {
                                SaleId = x.SaleId,
                                DateCreation = x.DateCreation,
                                CodeGuide = x.CodeGuide,
                                LocalName = x.Local.Name,
                                PriceTotal = x.PriceTotal,
                                EmployeeName = x.Employee.Name,
                                ClientName = x.Client.Name,
                                State = x.State,
                                StateDelivery = x.StateDelivery
                            }).ToList();

                    }
                    else
                    {

                        response.Result = context.Sale.Where(x => x.State == ConstantHelper.Status.ACTIVE)
                            .Select(x => new
                            {
                                SaleId = x.SaleId,
                                DateCreation = x.DateCreation,
                                CodeGuide = x.CodeGuide,
                                LocalName = x.Local.Name,
                                PriceTotal = x.PriceTotal,
                                EmployeeName = x.Employee.Name,
                                ClientName = x.Client.Name,
                                State = x.State,
                                StateDelivery = x.StateDelivery
                            }).ToList();

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

        [Authorize]
        [Route("sales")]
        [HttpPost]
        public HttpResponseMessage AddSale(SaleEntities model)
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
                    var sale = new Sale();
                    context.Sale.Add(sale);

                    sale.SaleId = model.saleId;
                    sale.DateCreation = DateTime.Today;
                    sale.CodeGuide = model.codeGuide;
                    sale.LocalId = model.localId;
                    sale.EmployeeId = model.EmployeeId;
                    sale.ClientId = model.clientId;
                    sale.State = ConstantHelper.Status.ACTIVE;
                    sale.StateDelivery = ConstantHelper.Status.ACTIVE;
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
        [Route("sales/{saleId}/items")]
        [HttpPost]
        public HttpResponseMessage AddSaleDetail(Int32 SaleId, SaleDetailEntities model)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                if (model == null || SaleId == null)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                    return Httpresponse;
                }
                else
                {
                    var sale = context.Sale.FirstOrDefault(x => x.SaleId == SaleId);
                    foreach (var sD in model.listProductSale) {

                        var saleDetail = new SaleDetail();
                        context.SaleDetail.Add(saleDetail);

                        saleDetail.SaleId = SaleId;
                        saleDetail.ProductId = sD.Item1;
                        saleDetail.Quantity = sD.Item2;
                        saleDetail.UnitPrice = sD.Item3;
                        saleDetail.PriceSubTotal = sD.Item4;
                        saleDetail.State = ConstantHelper.Status.ACTIVE;
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
        [Route("sales/{saleId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteSale(Int16 SaleId)
        {
            var Httpresponse = new HttpResponseMessage();
            try
            {
                if (SaleId == null)
                {
                    Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
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
                return Httpresponse;
            }
            catch
            {
                Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                return Httpresponse;
            }
        }

        [Authorize]
        [Route("sales")]
        [HttpPut]
        public HttpResponseMessage UpdateSale(SaleEntities model)
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

                    var sale = context.Sale.FirstOrDefault(x => x.State == ConstantHelper.Status.ACTIVE && x.SaleId == model.saleId);

                    sale.SaleId = model.saleId;
                    sale.CodeGuide = model.codeGuide;
                    sale.LocalId = model.localId;
                    sale.EmployeeId = model.EmployeeId;
                    sale.ClientId = model.clientId;
                    sale.StateDelivery = ConstantHelper.Status.ACTIVE;

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
