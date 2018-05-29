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
    }
}
