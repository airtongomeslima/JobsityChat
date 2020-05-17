using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace JobsityChat.Presentation.Helpers
{
    public class TokenAuth : AuthenticationSchemeOptions
    {
        public const string DefaultScemeName = "TokenAuthenticationScheme";
        public string TokenHeaderName { get; set; } = "Authorization";
    }

    public class MyCustomTokenAuthHandler : AuthenticationHandler<TokenAuth>
    {
        IConfiguration configuration;

        public MyCustomTokenAuthHandler(IOptionsMonitor<TokenAuth> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            this.configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.TokenHeaderName))
                return await Task.FromResult(AuthenticateResult.Fail($"Missing Header For Token: {Options.TokenHeaderName}"));

            var token = Request.Headers[Options.TokenHeaderName].ToString();

            var user = await UserInformationHelper.GetUserInfo(token, this.configuration["Authentication:BaseAddress"]);


            if (user == null)
            {
                return await Task.FromResult(AuthenticateResult.Fail($"User disconected."));
            }

            var username = user.UserName;
            var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Name, username),
                };
            var id = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(id);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
