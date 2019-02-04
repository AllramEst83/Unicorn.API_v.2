using Auth.API.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth.API.Exstensions
{
    public static class Exstensions
    {
        public static string GetClaimType(this ClaimsPrincipal principal, string type)
        {
            string claimType = String.Empty;

            switch (type)
            {
                case AuthServiceConstants.Name:
                    claimType = ClaimTypes.Name;
                    break;
                case AuthServiceConstants.Email:
                    claimType = ClaimTypes.Email;
                    break;
                case AuthServiceConstants.UserName:
                    claimType = ClaimTypes.GivenName;
                    break;
                case AuthServiceConstants.Admin:
                    claimType = ClaimTypes.Role;
                    break;
                case AuthServiceConstants.Expiration:
                    claimType = "exp";
                    break;
                default:
                    break;
            }

            return principal.Claims.Where(x => x.Type == claimType).Select(c => c.Value).SingleOrDefault();
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }


    }


}
