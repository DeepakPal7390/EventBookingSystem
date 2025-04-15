//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace EventBookingSystem.Middleware
//{
//    public class JwtMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly IConfiguration _configuration;

//        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
//        {
//            _next = next;
//            _configuration = configuration;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

//            if (token != null)
//            {
//                AttachUserToContext(context, token);
//            }

//            await _next(context);
//        }

//        private void AttachUserToContext(HttpContext context, string token)
//        {
//            try
//            {
//                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidIssuer = _configuration["Jwt:Issuer"],
//                    ValidAudience = _configuration["Jwt:Audience"],
//                    IssuerSigningKey = new SymmetricSecurityKey(key)
//                }, out var validatedToken);

//                context.User = principal;
//            }
//            catch
//            {

//                // If token is invalid, we can either return an error or do nothing
//            }
//        }
//    }
//}


// Middlewares/CustomAuthenticationHandler.cs
// Middlewares/JwtMiddleware.cs
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
            _logger.LogInformation("🛂 Authorization Header Received: {AuthHeader}", authHeader ?? "null");

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Replace("Bearer ", "");

                try
                {
                    // Resolve JwtSecurityTokenHandlerWrapper
                    var jwtHandler = context.RequestServices.GetRequiredService<JwtSecurityTokenHandlerWrapper>();
                    var claimsPrincipal = jwtHandler.ValidateJwtToken(token);

                    context.User = claimsPrincipal;
                    _logger.LogInformation("✅ JWT token validated and HttpContext.User set");
                }
                catch (SecurityTokenValidationException ex)
                {
                    _logger.LogError(ex, "❌ Token validation failed");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid or missing token.");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Unexpected error during JWT validation");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Internal Server Error");
                    return;
                }
            }
            else
            {
                _logger.LogWarning("❌ Missing or invalid Authorization header");
            }

            await _next(context);
        }
    }
}