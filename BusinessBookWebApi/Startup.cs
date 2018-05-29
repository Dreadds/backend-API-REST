using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Linq;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using System.Security.Claims;
using BusinessBookWebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using BusinessBookWebApi.Models;
using BusinessBookWebApi.Logics;

[assembly:OwinStartup(typeof(BusinessBookWebApi.Startup))]
namespace BusinessBookWebApi { 
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<OwinAuthDbContext>(() => new OwinAuthDbContext());
            app.CreatePerOwinContext<UserManager<IdentityUser>>(CreateManager);

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                AllowInsecureHttp = true,

            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
        {
            public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                context.Validated();
            }
            public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                var businessbookentities = new BusinessBookEntities();
                var Listemployee = businessbookentities.Employee.ToList();
            
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                foreach (var employee in Listemployee)
                {
                    var password = BusinessBookWebApi.Logics.CipherLogic.Cipher(CipherAction.Decrypt, CipherType.UserPassword, employee.Password);
                    if (context.UserName == employee.Users && context.Password == password)
                    {
                        identity.AddClaim(new Claim("username", employee.Users));
                        identity.AddClaim(new Claim("password", employee.Password));
                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("Invalid grant", "verifique error");
                        return;
                    }
                }
            }
        }

        private static UserManager<IdentityUser> CreateManager(IdentityFactoryOptions<UserManager<IdentityUser>> options, IOwinContext context)
        {
            var userStore = new UserStore<IdentityUser>(context.Get<OwinAuthDbContext>());
            var owinManager = new UserManager<IdentityUser>(userStore);
            return owinManager;
        }
    }
}
