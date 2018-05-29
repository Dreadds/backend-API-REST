using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Entities
{
    public class TokenEntities
    {
        [JsonProperty("access_token")]
        public string accessToken { get; set; }
        [JsonProperty("token_type")]
        public string tokenType { get; set; }
        [JsonProperty("expires_in")]
        public int expiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string refreshToken { get; set; }
        [JsonProperty("error")]
        public string error { get; set; }
        [JsonProperty("issued")]
        public DateTime issued { set; get; }
        [JsonProperty("expires")]
        public DateTime expires { set; get; }
    }
}