using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;

namespace WebApp_MSSTS_DotNet
{
    public partial class Startup
    {

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = "cb4b16f7-304e-4195-8ac1-ee9b068dee93",
                    Authority = "https://login.windows-ppe.net/common/v2.0",
                    PostLogoutRedirectUri = "https://localhost:44327/",
                    
                    // For MS STS, send scope=openid
                    Scope = "openid",

                    // Treat as multi-tenant, disable issuer validation.
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters {
                        ValidateIssuer = false
                    },

                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Error?message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        }
                    }
                });
        }
    }
}