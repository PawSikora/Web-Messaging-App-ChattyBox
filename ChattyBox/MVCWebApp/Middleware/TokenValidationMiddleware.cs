using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace MVCWebApp.Middleware
{
    public class TokenValidationMiddleware  :IMiddleware
    {
        private readonly IConfiguration _configuration;
        public TokenValidationMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var controllerName = context.Request.RouteValues["controller"]?.ToString();
            var actionName = context.Request.RouteValues["action"]?.ToString();

            if (actionName == "Login" || actionName == "Register" && controllerName == "User")
            {
                await next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = ValidateToken(token);
            if (userId is not null)
            {
                context.Items["userId"] = userId;
                await next(context);
                return;
            }
            await next(context);
            return;
        }


        private int? ValidateToken(string token)
        {
            if (token is null)
                return null;


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
            catch (SecurityTokenException)
            {
                return null;
            }
        }
    }
}
