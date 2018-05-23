using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace BusinessBookWebApi.Models
{
    public class Response
    {
        public HttpStatusCode Code { set; get; }
        public String Message { set; get; }
        public Object Result { set; get; } = null;
    }
}