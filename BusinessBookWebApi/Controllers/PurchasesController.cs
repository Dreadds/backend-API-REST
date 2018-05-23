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
    [RoutePrefix("purchases")]
    public class PurchasesController : BaseApiController
    {
        [HttpGet]
        [Route("{purchaseId}")]
        public HttpResponseMessage ListPurchasess(Int32? PurchaseId)
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
                    if (PurchasesId.HasValue)
                    {

                        response.Result = context.Purchase.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.PurchaseId == PurchaseId)
                            .Select(x => new
                            {
                                PurchaseId = x.PurchaseId,
                                DateCreation = x.DateCreation,
                                CodeGuide = x.CodeGuide,
                                LocalName = x.Local.Name,
                                PriceTotal = x.PriceTotal,
                                ProveedorName = x.Proveedor.Name,
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
                                ProveedorName = x.Proveedor.Name,
                                State = x.State
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
    }
}
