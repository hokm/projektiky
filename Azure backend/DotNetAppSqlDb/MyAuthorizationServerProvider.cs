using DotNetAppSqlDb.Models;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace DotNetAppSqlDb
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //klient bol validovaný
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);


            var user = db.Users.SingleOrDefault(u => u.Email == context.UserName && u.Password == context.Password);
            if(user != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.FullName));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password are incorrect.");
            }     
        }
    }
}