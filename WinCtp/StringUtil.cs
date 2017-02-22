using System;
using System.Security.Cryptography;
using System.Text;

namespace WinCtp
{
    internal static class StringUtil
    {
        public static string Protect(this string src)
        {
            var data = Encoding.UTF8.GetBytes(src);
            var dpapi = new DpapiDataProtector(HardwareInfo.CupId, "CTP.NET") { Scope = DataProtectionScope.LocalMachine };
            var bts = dpapi.Protect(data);
            return Convert.ToBase64String(bts);
        }

        public static string Unprotect(this string src)
        {
            byte[] bts;
            try
            {
                bts = Convert.FromBase64String(src);
            }
            catch (FormatException)
            {
                return src;
            }
            
            try
            {
                var dpapi = new DpapiDataProtector(HardwareInfo.CupId, "CTP.NET")
                {
                    Scope = DataProtectionScope.LocalMachine
                };
                var data = dpapi.Unprotect(bts);
                return Encoding.UTF8.GetString(data);
            }
            catch (CryptographicException)
            {
                return src;
            }
        }
    }
}