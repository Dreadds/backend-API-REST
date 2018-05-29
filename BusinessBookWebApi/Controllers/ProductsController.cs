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
    public class ProductsController : BaseApiController
    {
        [Authorize]
        [Route("product")]
        [Route("product/{productId}")]
        [HttpGet]
        public HttpResponseMessage ListProduct(Int32? ProductId)
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

                    if (ProductId.HasValue)
                    {

                        response.Result = context.Product.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.ProductId == ProductId)
                            .Select(x => new
                            {
                                productId = x.ProductId,
                                name = x.Name,
                                unitPrice = x.UnitPrice,
                                state = x.State
                            }).ToList();

                    }
                    else
                    {

                        response.Result = context.Product.Where(x => x.State == ConstantHelper.Status.ACTIVE)
                            .Select(x => new
                            {
                                productId = x.ProductId,
                                name = x.Name,
                                unitPrice = x.UnitPrice,
                                state = x.State
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
