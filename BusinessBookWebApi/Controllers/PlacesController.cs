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
    public class PlacesController : BaseApiController
    {
        //List District
        
        [Route("districts")]
        [Route("districts/{districtId}")]
        [HttpGet]
        public HttpResponseMessage ListDistrict(Int32? DistrictId = null)
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
                    response.Result = null;
                    return Httpresponse;
                }
                else
                {

                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Code = HttpStatusCode.OK;
                    response.Message = "success";

                    if (DistrictId.HasValue)
                    {

                        response.Result = context.District.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.DistrictId == DistrictId)
                            .Select(x => new
                            {
                                districtId = x.DistrictId,
                                name = x.Name,
                                state = x.State
                            }).ToList();

                    }
                    else
                    {

                        response.Result = context.District.Where(x => x.State == ConstantHelper.Status.ACTIVE)
                            .Select(x => new
                            {
                                districtId = x.DistrictId,
                                name = x.Name,
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

        //List Province
        
        [Route("provinces")]
        [Route("provinces/{provinceId}")]
        [HttpGet]
        public HttpResponseMessage ListProvince(Int32? ProvinceId = null)
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
                    response.Result = null;
                    return Httpresponse;
                }
                else
                {

                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Code = HttpStatusCode.OK;
                    response.Message = "success";

                    if (ProvinceId.HasValue)
                    {

                        response.Result = context.Province.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.ProvinceId == ProvinceId)
                            .Select(x => new
                            {
                                provinceId = x.ProvinceId,
                                name = x.Name,
                                state = x.State
                            }).ToList();

                    }
                    else
                    {

                        response.Result = context.Province.Where(x => x.State == ConstantHelper.Status.ACTIVE)
                            .Select(x => new
                            {
                                provinceId = x.ProvinceId,
                                name = x.Name,
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

        //List Dire
        [Route("locations")]
        [Route("locations/{locationId}")]
        [HttpGet]
        public HttpResponseMessage ListLocation(Int32? LocationId = null)
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
                    response.Result = null;
                    return Httpresponse;
                }
                else
                {

                    Httpresponse = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Code = HttpStatusCode.OK;
                    response.Message = "success";

                    if (LocationId.HasValue)
                    {

                        response.Result = context.Location.Where(x => x.State == ConstantHelper.Status.ACTIVE && x.LocationId == LocationId)
                            .Select(x => new
                            {
                                locationId = x.LocationId,
                                name = x.Name,
                                state = x.State
                            }).ToList();

                    }
                    else
                    {

                        response.Result = context.Location.Where(x => x.State == ConstantHelper.Status.ACTIVE)
                            .Select(x => new
                            {
                                locationId = x.LocationId,
                                name = x.Name,
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
