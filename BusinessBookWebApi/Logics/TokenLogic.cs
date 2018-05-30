using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessBookWebApi.Logics
{
    public class TokenLogic
    {
        //expiration time = 2 horas
        public static bool ValidateToken(string token, int maxValidHours = 365 * 4320)
        {
            try
            {
                token = CipherLogic.Cipher(CipherAction.Decrypt, CipherType.Token, token);

                var data = Convert.FromBase64String(token);
                var createdAt = DateTime.FromBinary(BitConverter.ToInt64(data, 0));

                return createdAt > DateTime.UtcNow.AddHours(maxValidHours * -1);
            }
            catch
            {
                return false;
            }
        }
    }
}