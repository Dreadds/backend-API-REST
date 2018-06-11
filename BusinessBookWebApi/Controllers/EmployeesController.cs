using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessBookWebApi.Entities;
using System.Transactions;
using BusinessBookWebApi.Models;
using BusinessBookWebApi.Helpers;
using BusinessBookWebApi.Logics;
using System.Security.Claims;
using BusinessBookWebApi;
using Microsoft.Owin.Security.OAuth;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace BusinessBookWebApi.Controllers
{
    
    [RoutePrefix("businessbookapi/v1")]
    public class EmployeesController : BaseApiController
    {
        [AllowAnonymous]
        [Route("lista")]
        [HttpGet]
        public HttpResponseMessage lista(LoginEntities model)
        {
            var HttpResponse = new HttpResponseMessage();
            try
            {

                response.Code = HttpStatusCode.OK;
                response.Message = "Success";
                response.Result = context.Location.Where(x => x.State == ConstantHelper.Status.ACTIVE).Select(x => x.Name).ToList();

                HttpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return HttpResponse;
            }
            catch
            {
                HttpResponse = new HttpResponseMessage(HttpStatusCode.Conflict);
                return HttpResponse;
            }
        }
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public HttpResponseMessage LoginEmployee(LoginEntities model)
        {
            var HttpResponse = new HttpResponseMessage();
            try
            {
                // IF MODEL IS NULL
                if(model == null)
                {
                    HttpResponse = new HttpResponseMessage(HttpStatusCode.NoContent);
                    response.Code = HttpStatusCode.NoContent;
                    response.Message = "No Content";
                    response.Result = null;

                    HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return HttpResponse;
                }
                Employee employee = new Employee();

                //VALIDATE DATA
                if (!String.IsNullOrEmpty(model.users) || !String.IsNullOrEmpty(model.password))
                {
                    employee = context.Employee.FirstOrDefault(x => x.Users == model.users && x.State == ConstantHelper.Status.ACTIVE);

                    //EMPLOYEE DO NOT EXIST
                    if (employee == null)
                    {
                        HttpResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                        response.Code = HttpStatusCode.NotFound;
                        response.Message = "Not Found";
                        response.Result = null;

                        HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                        HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return HttpResponse;
                    }

                    var password = CipherLogic.Cipher(CipherAction.Decrypt, CipherType.UserPassword, employee.Password);

                    //COMPARATION PASSWORD
                    if (password == model.password)
                    {
                        //DOMAIN 
                        String baseAddress = "http://chemita96-001-site1.dtempurl.com";
                        //String baseAddress = "http://localhost:16669";
                        //CREATE A NEW TOKEN FOR EMPLOYEE
                        if (!employee.TokenEmployeeId.HasValue)
                        {
                            var fecha = DateTime.Now.AddHours(-7);
                            TokenEntities tokenEntities = new TokenEntities();
                            using (var client = new HttpClient())
                            {
                                var form = new Dictionary<string, string>
                                {
                                    {"grant_type", "password"},
                                    {"username", employee.Users},
                                    {"password", password},
                                };
                                var tokenResponse = client.PostAsync(baseAddress + "/oauth/token", new FormUrlEncodedContent(form)).Result;
                                //CONVERT 
                                tokenEntities = tokenResponse.Content.ReadAsAsync<TokenEntities>(new[] { new JsonMediaTypeFormatter() }).Result;
                                if (tokenEntities.accessToken == null)
                                {
                                    HttpResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                                    response.Code = HttpStatusCode.BadRequest;
                                    response.Message = "Bad Request";
                                    response.Result = null;

                                    HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                                    HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                    return HttpResponse;
                                }
                                else
                                {
                                    //IF TOKEN IS NULL ADD KEY AND SAVE IN DATA BASE
                                    var tokenEmployee = new TokenEmployee();
                                    context.TokenEmployee.Add(tokenEmployee);
                                    tokenEmployee.AccessToken = tokenEntities.accessToken;
                                    tokenEmployee.ExpireInToken = tokenEntities.expiresIn;
                                    tokenEmployee.ErrorToken = tokenEntities.error;
                                    tokenEmployee.TypeToken = tokenEntities.tokenType;
                                    tokenEmployee.RefreshToken = tokenEntities.refreshToken;
                                    tokenEmployee.Issued = fecha;
                                    tokenEmployee.Expires = fecha.AddHours(24);
                                    tokenEmployee.State = ConstantHelper.Status.ACTIVE;
                                    context.SaveChanges();
                                    //LINK EMPLOYEE WITH TOKEN
                                    employee.TokenEmployeeId = tokenEmployee.TokenEmployeeId;
                                    context.SaveChanges();
                                }
                            }

                            //SHOW HOW TO JSON 
                            token.accessToken = tokenEntities.accessToken;
                            token.tokenType = tokenEntities.tokenType;
                            token.expiresIn = tokenEntities.expiresIn;
                            token.refreshToken = tokenEntities.refreshToken;
                            token.username = employee.Users;
                            token.issued = fecha;
                            token.expires = fecha.AddHours(24);

                            response.Code = HttpStatusCode.OK;
                            response.Message = "Success";
                            response.Result = token;

                            //RESULT
                            HttpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                            HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                            HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        }
                        else if (employee.TokenEmployeeId.HasValue)
                        {
                            //verificar si el token esta activo 
                            bool verificar = ValidateToken(employee.TokenEmployee.AccessToken);
                            if (!verificar)
                            {
                                var tokenString = employee.TokenEmployee.AccessToken;
                                var employeeData = context.Employee.FirstOrDefault(x => x.TokenEmployee.AccessToken == tokenString);
                                var pwd = CipherLogic.Cipher(CipherAction.Decrypt, CipherType.UserPassword, employee.Password);
                                var newToken = GeneretaToken(employeeData.Users, pwd);
                                var tokenEmployee = context.TokenEmployee.FirstOrDefault(x => x.AccessToken == newToken);

                                token.accessToken = tokenEmployee.AccessToken;
                                token.tokenType = tokenEmployee.TypeToken;
                                token.expiresIn = tokenEmployee.ExpireInToken;
                                token.refreshToken = tokenEmployee.RefreshToken;
                                token.username = employeeData.Users;
                                token.issued = tokenEmployee.Issued;
                                token.expires = tokenEmployee.Expires;

                            }
                            {
                                token.accessToken = employee.TokenEmployee.AccessToken;
                                token.tokenType = employee.TokenEmployee.TypeToken;
                                token.expiresIn = employee.TokenEmployee.ExpireInToken;
                                token.refreshToken = employee.TokenEmployee.RefreshToken;
                                token.username = employee.Users;
                                token.issued = employee.TokenEmployee.Issued;
                                token.expires = employee.TokenEmployee.Expires;
                            }
                            response.Code = HttpStatusCode.OK;
                            response.Message = "Success";
                            response.Result = token;

                            HttpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                            HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                            HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        }
                    }
                }
                
                //EMPLOYEE WAS DELETE       
                if(employee.State == ConstantHelper.Status.INACTIVE)
                {
                    HttpResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = "Not Found";
                    response.Result = null;

                    HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return HttpResponse;
                }
                return HttpResponse;
            }
            catch(Exception ex)
            {
                HttpResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                response.Message = "Bad Gateway";
                response.Result = null;
                HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return HttpResponse;
            }
        }

        [AllowAnonymous]
        [Route("registeremployee")]
        [HttpPost]
        public HttpResponseMessage RegisterEmployee(EmployeeEntities model)
        {
            var HttpResponse = new HttpResponseMessage();
            try
            {
                using (var ts = new TransactionScope())
                {
                    //IF MODEL IS NULL
                    if (model == null)
                    {
                        HttpResponse = new HttpResponseMessage(HttpStatusCode.NoContent);
                        response.Code = HttpStatusCode.NoContent;
                        response.Message = "No Content";
                        response.Result = null;
                        
                        HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                        HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return HttpResponse;
                    }
                    //IF EMPLOYEEID HAVE 0
                    if (model.employeeId == 0)
                    {

                        HttpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        response.Code = HttpStatusCode.BadRequest;
                        response.Message = "Bad Request";
                        response.Result = null;

                        HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                        HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return HttpResponse;
                    }
                    
                    //IF USERNAME EXIST
                    var listEmployee = context.Employee.ToList();
                    foreach (var employeee in listEmployee)
                    {
                        if (employeee.Users == model.users)
                        {
                            HttpResponse = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                            response.Code = HttpStatusCode.NotAcceptable;
                            response.Message = "Not Acceptable";
                            response.Result = null;

                            HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                            HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            return HttpResponse;
                        }
                    }
                    
                    //CREATE EMPLOYEE
                    var employee = new Employee();
                    if (model.employeeId != 0)
                    {
                        context.Employee.Add(employee);
                    }
                    employee.DateCreation = DateTime.Today;
                    employee.DateUpdate = DateTime.Today;
                    employee.State = ConstantHelper.Status.ACTIVE;
                    employee.Users = model.users;

                    var password = CipherLogic.Cipher(CipherAction.Encrypt, CipherType.UserPassword, model.password);
                    employee.Password = password;
                    context.SaveChanges();


                    ts.Complete();

                    //INSERT TO LOGIN
                    /*var loginResult = this.LoginEmployee(new LoginEntities()
                    {
                        users = model.users,
                        password = model.password
                    });*/

                    response.Code = HttpStatusCode.OK;
                    response.Message = "Success";
                    response.Result = null;

                    HttpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return HttpResponse;
                }


            }
            catch (Exception e)
            {
                HttpResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                response.Message = "Bad Gateway";
                response.Result = null;
                HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return HttpResponse;

            }
        }
        
        [Route("registercompany")]
        [HttpPost]
        public HttpResponseMessage RegisterCompany(CompanyEntities model)
        {
            var HttpResponse = new HttpResponseMessage();
            try
            {
                using (var ts = new TransactionScope())
                {
                    var id = GetEmployeeId();

                    if (!id.HasValue)
                    {
                        HttpResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                        response.Code = HttpStatusCode.Unauthorized;
                        response.Message = "Authorization has been denied for this request.";
                        response.Result = null;
                        HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                        HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return HttpResponse;
                    }
                    else
                    {
                        //FI MODEL IS NULL
                        if (model == null)
                        {
                            HttpResponse = new HttpResponseMessage(HttpStatusCode.NoContent);
                            response.Code = HttpStatusCode.NoContent;
                            response.Message = "No Content";
                            response.Result = null;
                            HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                            HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            return HttpResponse;
                        }

                        if (model.companyId == 0)
                        {
                            HttpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            response.Code = HttpStatusCode.BadRequest;
                            response.Message = "Bad Request";
                            response.Result = null;
                            HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                            HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            return HttpResponse;
                        }

                        //IF NAME IS SIMILAR TO ANOTHER USER
                        var listCompany = context.Company.ToList();
                        foreach (var companyy in listCompany)
                        {
                            if (companyy.Name == model.name)
                            {
                                HttpResponse = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                                response.Code = HttpStatusCode.NotAcceptable;
                                response.Message = "Not Acceptable";
                                response.Result = null;
                                HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                                HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                return HttpResponse;
                            }
                        }
                        
                        var company = new Company();
                        if (model.companyId != 0)
                        {
                            context.Company.Add(company);
                        }

                        company.Name = model.name;
                        company.state = ConstantHelper.Status.ACTIVE;
                        company.LocationId = model.locationId;
                        company.EmployeeId = model.employeeId;

                        context.SaveChanges();

                        ts.Complete();

                        HttpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                        response.Code = HttpStatusCode.OK;
                        response.Message = "Success";
                        response.Result = null;
                    }

                    HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                    HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    return HttpResponse;
                }
            }
            catch
            {
                HttpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Code = HttpStatusCode.BadRequest;
                response.Message = "Bad Request";
                response.Result = null;
                HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return HttpResponse;
            }
        }
        
        [Route("viewcompany/{companyId}")]
        [HttpGet]
        public HttpResponseMessage ViewCompany(Int32? CompanyId = null)
        {
            var HttpResponse = new HttpResponseMessage();
            try
            {
                using (var ts = new TransactionScope())
                {
                    var id = GetEmployeeId();

                    if (!id.HasValue)
                    {
                        HttpResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                        response.Code = HttpStatusCode.Unauthorized;
                        response.Message = "Authorization has been denied for this request.";
                        response.Result = null;
                        HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                        HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return HttpResponse;
                    }
                    else
                    {
                        //FI MODEL IS NULL
                        if (CompanyId == null)
                        {
                            HttpResponse = new HttpResponseMessage(HttpStatusCode.NoContent);
                            response.Code = HttpStatusCode.NoContent;
                            response.Message = "No Content";
                            response.Result = null;
                            HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                            HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            return HttpResponse;
                        }

                        var company = new Company();

                        company = context.Company.FirstOrDefault(x => x.CompanyId == CompanyId);

                        if (company == null)
                        {
                            HttpResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
                            response.Code = HttpStatusCode.NotFound;
                            response.Message = "Not Found";
                            response.Result = null;
                            HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                            HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            return HttpResponse;
                        }

                        if (company.state == ConstantHelper.Status.INACTIVE)
                        {
                            HttpResponse = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                            response.Code = HttpStatusCode.NotAcceptable;
                            response.Message = "Not Acceptable";
                            response.Result = null;
                            HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                            HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            return HttpResponse;
                        }

                        response.Code = HttpStatusCode.OK;
                        response.Message = "success";
                        response.Result = company;

                        ts.Complete();

                       
                        HttpResponse = new HttpResponseMessage(HttpStatusCode.OK);
                        HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                        HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    }
                    return HttpResponse;
                }
            }
            catch
            {
                HttpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Code = HttpStatusCode.BadRequest;
                response.Message = "Bad Request";
                response.Result = null;
                HttpResponse.Content = new StringContent(JsonConvert.SerializeObject(response));
                HttpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return HttpResponse;
            }
        }

        public void Login(LoginEntities model)
        {
            if (!String.IsNullOrEmpty(model.users) || !String.IsNullOrEmpty(model.password))
            {
                var employee = context.Employee.FirstOrDefault(x => x.Users == model.users && x.State == ConstantHelper.Status.ACTIVE);

                
                var password = CipherLogic.Cipher(CipherAction.Decrypt, CipherType.UserPassword, employee.Password);

                //COMPARATION PASSWORD
                if (password == model.password)
                {
                    //DOMAIN 
                    String baseAddress = "http://chemita96-001-site1.dtempurl.com";
                    //String baseAddress = "http://localhost:16669";
                    //CREATE A NEW TOKEN FOR EMPLOYEE
                    if (!employee.TokenEmployeeId.HasValue)
                    {
                        var fecha = DateTime.Now.AddHours(-7);
                        TokenEntities tokenEntities = new TokenEntities();
                        using (var client = new HttpClient())
                        {
                            var form = new Dictionary<string, string>
                                {
                                    {"grant_type", "password"},
                                    {"username", employee.Users},
                                    {"password", password},
                                };
                            var tokenResponse = client.PostAsync(baseAddress + "/oauth/token", new FormUrlEncodedContent(form)).Result;
                            //CONVERT 
                            tokenEntities = tokenResponse.Content.ReadAsAsync<TokenEntities>(new[] { new JsonMediaTypeFormatter() }).Result;
                            if (tokenEntities.accessToken != null)
                            {
                                //IF TOKEN IS NULL ADD KEY AND SAVE IN DATA BASE
                                var tokenEmployee = new TokenEmployee();
                                context.TokenEmployee.Add(tokenEmployee);
                                tokenEmployee.AccessToken = tokenEntities.accessToken;
                                tokenEmployee.ExpireInToken = tokenEntities.expiresIn;
                                tokenEmployee.ErrorToken = tokenEntities.error;
                                tokenEmployee.TypeToken = tokenEntities.tokenType;
                                tokenEmployee.RefreshToken = tokenEntities.refreshToken;
                                tokenEmployee.Issued = fecha;
                                tokenEmployee.Expires = fecha.AddHours(24);
                                tokenEmployee.State = ConstantHelper.Status.ACTIVE;
                                context.SaveChanges();
                                //LINK EMPLOYEE WITH TOKEN
                                employee.TokenEmployeeId = tokenEmployee.TokenEmployeeId;
                                context.SaveChanges();
                            }
                        }

                        //SHOW HOW TO JSON 
                        token.accessToken = tokenEntities.accessToken;
                        token.tokenType = tokenEntities.tokenType;
                        token.expiresIn = tokenEntities.expiresIn;
                        token.refreshToken = tokenEntities.refreshToken;
                        token.username = employee.Users;
                        token.issued = fecha;
                        token.expires = fecha.AddHours(24);
                    }
                }
            }
        }
    }
}
