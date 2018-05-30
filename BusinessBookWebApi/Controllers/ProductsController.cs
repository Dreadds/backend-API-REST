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
    public class ProductsController : BaseApiController
    {
        [Authorize]
        [Route("products")]
        [Route("products/{productId}")]
        [HttpGet]
        public HttpResponseMessage ListProducts(Int32? ProductId = null)
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
        [Authorize]
        [Route("products")]
        [HttpPost]
        public HttpResponseMessage AddProduct(ProductEntities model)
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

                        List<Local> LstLocal = new List<Local>();
                        var company = context.Company.FirstOrDefault(x => x.EmployeeId == id);
                        LstLocal = context.Local.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.CompanyId == company.CompanyId).ToList();
                        if(LstLocal == null)
                        {
                            Httpresponse = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                            return Httpresponse;
                        }

                        var product = new Product();
                        context.Product.Add(product);
                        
                        product.Name = model.name;
                        product.UnitPrice = model.unitPrice;
                        product.State = ConstantHelper.Status.ACTIVE;

                        foreach (var local in LstLocal)
                        {
                            var inventary = new Inventory();
                            context.Inventory.Add(inventary);
                            inventary.ProductId = product.ProductId;
                            inventary.Quantity = 0;
                            inventary.DateUpdate = DateTime.Now;
                            inventary.State = ConstantHelper.Status.ACTIVE;
                            inventary.LocalId = local.LocalId;
                            context.SaveChanges();
                        }
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
        [Route("products/{productId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteProduct(Int16 ProductId)
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
                    if (ProductId == null)
                    {
                        Httpresponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                        return Httpresponse;
                    }
                    else
                    {
                        var product = new Product();
                        product = context.Product.FirstOrDefault(x => x.ProductId == ProductId);
                        product.State = ConstantHelper.Status.INACTIVE;
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
        [Route("products")]
        [HttpPut]
        public HttpResponseMessage UpdateProduct(ProductEntities model)
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

                        var product = context.Product.FirstOrDefault(x => x.State == ConstantHelper.Status.ACTIVE && x.ProductId == model.productId);
                        
                        product.Name = model.name;
                        product.UnitPrice = model.unitPrice;

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
