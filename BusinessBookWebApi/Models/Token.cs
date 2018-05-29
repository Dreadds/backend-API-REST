using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace BusinessBookWebApi
{
    public class Token
    {
        public String accessToken { get; set; }
        public String tokenType { get; set; }
        public Int32? expiresIn { get; set; }
        public String refreshToken { get; set; }
        public String username { set; get; }
        public DateTime? issued { set; get; }
        public DateTime? expires { set; get; }
    }
}