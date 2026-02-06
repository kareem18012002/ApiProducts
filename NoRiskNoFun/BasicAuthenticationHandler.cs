using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace NoRiskNoFun
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

            var authHeader = Request.Headers["Authorization"].ToString();

            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.Fail("Unknown Scheme"));

            var encodedCredentials = authHeader["Basic ".Length..];
            var decodedCredentials =
                System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

            var usernameAndPassword = decodedCredentials.Split(':', 2);

            if (usernameAndPassword.Length != 2)
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));

            var username = usernameAndPassword[0];
            var password = usernameAndPassword[1];

            if (username == "admin" && password == "password")
            {
                var claims = new[]
                {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, "1")
        };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
        }

    }
}