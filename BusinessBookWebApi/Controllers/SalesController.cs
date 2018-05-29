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
    public class SalesController : BaseApiController
    {
        [Authorize]
        [HttpGet]
        [Route("sales")]
        //[Route("sales/{saleId}")]
        public HttpResponseMessage ListSales()
        {
            Int32? SaleId = null;
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
    }
}
