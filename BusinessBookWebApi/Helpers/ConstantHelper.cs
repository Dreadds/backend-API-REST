using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Text;

namespace BusinessBookWebApi.Helpers
{
    public class ConstantHelper
    {
        public static class Status {
            public const string ACTIVE = "ACT";
            public const string INACTIVE = "INA";
            public const string FINISH = "FIN";
            public const string CANCEL = "CAN";
        }
        public static class Gender
        {
            public const String MALE = "M";
            public const String FEMALE = "F";
        }

        public static class Rol
        {
            public const String ADMIN = "ADMIN";
        }

        public const String UserSession = "USERSESSION";

        public const string TOKEN_HEADER_NAME = "Authorization";

        public const int TOKEN_TIMEOUT = 24;

        public static string GetDataConfiguration(string key) => string.IsNullOrEmpty(key) ? string.Empty : ConfigurationManager.AppSettings[key].ToString();
        public static string UniqueShortIdentifier => Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-", StringComparison.Ordinal)).ToUpper();
    }
}
