using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace EventBookingSystem.Middleware
{
    public class JwtSecurityTokenHandlerWrapper
    {
        private readonly RSA _rsa;
        private readonly ILogger<JwtSecurityTokenHandlerWrapper> _logger;

        public JwtSecurityTokenHandlerWrapper(ILogger<JwtSecurityTokenHandlerWrapper> logger)
        {
            _logger = logger;
            var publicKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "Keys", "public.pem");
            _logger.LogInformation(" Reading public key from: {PublicKeyPath}", publicKeyPath);
            var publicKeyText = File.ReadAllText(publicKeyPath);
            _rsa = RSA.Create();
            try
            {
                _rsa.ImportFromPem(publicKeyText.ToCharArray());
                _logger.LogInformation("Public key imported successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Failed to import public key from {PublicKeyPath}", publicKeyPath);
                throw;
            }
        }

        public ClaimsPrincipal ValidateJwtToken(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(_rsa),
                RoleClaimType = "role",
                NameClaimType = "email"
            };

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            try
            {
                var principal = handler.ValidateToken(token, validationParameters, out var securityToken);
                _logger.LogInformation("Token validated successfully");
                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation failed");
                throw;
            }
        }
    }
}
