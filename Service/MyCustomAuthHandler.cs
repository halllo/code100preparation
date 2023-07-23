using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Service
{
    public class MyCustomAuthSchemeOptions : AuthenticationSchemeOptions
    { }

    public class MyCustomAuthHandler : AuthenticationHandler<MyCustomAuthSchemeOptions>
    {
        public MyCustomAuthHandler(
            IOptionsMonitor<MyCustomAuthSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var auth = base.Request.Headers.Authorization.ToString();
            var apikey = auth.StartsWith("apikey ", StringComparison.InvariantCultureIgnoreCase) ? auth.Substring("apikey ".Length) : auth;

            if (string.IsNullOrEmpty(apikey))
            {
                return Task.FromResult(AuthenticateResult.Fail("no apikey"));
            }
            else
            {
                var claims = new[] { new Claim("apikey", apikey) };
                var claimsIdentity = new ClaimsIdentity(claims, nameof(MyCustomAuthHandler));
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
        }
    }
}