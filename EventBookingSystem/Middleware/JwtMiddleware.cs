using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventBookingSystem.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            _logger.LogInformation("Authorization Header Received: {AuthHeader}", authHeader ?? "null");

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Replace("Bearer ", "");

                try
                {
                    var jwtHandler = context.RequestServices.GetRequiredService<JwtSecurityTokenHandlerWrapper>();
                    var claimsPrincipal = jwtHandler.ValidateJwtToken(token);

                    context.User = claimsPrincipal;
                    _logger.LogInformation("JWT token validated and HttpContext.User set");

                    var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                    if (string.IsNullOrEmpty(userId))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Unauthorized: userId claim missing in token.");
                        return;
                    }

                    context.Items["UserId"] = userId;
                    _logger.LogInformation("userId claim extracted and stored: {UserId}", userId);

                    
                    var role = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                    if (!string.IsNullOrEmpty(role))
                    {
                        var identity = (ClaimsIdentity)context.User.Identity;
                        identity.AddClaim(new Claim(ClaimTypes.Role, role));
                        _logger.LogInformation("Role claim extracted and added as ClaimTypes.Role: {Role}", role);
                    }
                    else
                    {
                        _logger.LogWarning("JWT token is missing role claim");
                    }
                }
                catch (SecurityTokenValidationException ex)
                {
                    _logger.LogError(ex, "Token validation failed");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid or missing token.");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error during JWT validation");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Internal Server Error");
                    return;
                }
            }
            else
            {
                _logger.LogWarning("Missing or invalid Authorization header");
            }

            await _next(context);
        }
    }
}



