﻿// Middlewares/CustomAuthenticationHandler.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace EventBookingSystem.Middleware
{
    public class PassThroughAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public PassThroughAuthenticationHandler(
             IOptionsMonitor<AuthenticationSchemeOptions> options,
             ILoggerFactory logger,
             UrlEncoder encoder)
             : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // JwtMiddleware sets HttpContext.User; just pass through
            if (Context.User?.Identity?.IsAuthenticated == true)
            {
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(Context.User, Scheme.Name)));
            }
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
