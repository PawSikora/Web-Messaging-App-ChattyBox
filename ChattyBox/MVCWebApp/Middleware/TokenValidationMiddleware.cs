using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MVCWebApp.Middleware
{
    public class TokenValidationMiddleware : IMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenValidationMiddleware> _logger;

        public TokenValidationMiddleware(IConfiguration configuration, ILogger<TokenValidationMiddleware> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Cookies["userToken"];

            if (!string.IsNullOrEmpty(token))
            {
                var userId = ValidateToken(token);

                if (userId.HasValue)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()),
                    };

                    var identity = new ClaimsIdentity(claims, "cookie");

                    var principal = new ClaimsPrincipal(identity);

                    context.User = principal;
                }
                else
                {
                    context.Request.Path = "/User/Logout";
                }
            }

            await next(context);
        }

        private int? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenSettings:SecurityKey"]));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["TokenSettings:Issuer"],
                    ValidAudience = _configuration["TokenSettings:Audience"],
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "userId").Value);
                return userId;
            }
            catch(SecurityTokenExpiredException ex)
            {
                _logger.LogInformation(ex, "Token expired");
                return null;
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogCritical(ex, "Security token exception");
                return null;
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex, "Token validation failed: {ErrorMessage}", ex.Message);
                return null;
            }
        }
    }
}
