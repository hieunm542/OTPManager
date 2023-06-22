using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JwtAuthDemo.Infrastructure;
using Microsoft.AspNetCore.Http;
using OTPManager.Models;

public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    public JwtAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, DBContext _dbContext)
    {
        // Perform your authentication logic here

        string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
        if (!string.IsNullOrEmpty(token))
        {
            JwtPayload principal = DecodeJwtToken(token);
            var system = principal[ClaimTypes.System];
            if (system.ToString() == TokenTypes.CustomToken.ToString())
            {
                Console.WriteLine("CheckTOken");
                var tokenQuery = _dbContext.Tokens.FirstOrDefault(t=>t.TokenString == token);
                if (tokenQuery == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

            }
        }
        

        // Call the next middleware in the pipeline
        await _next(context);
    }
    public static JwtPayload DecodeJwtToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        return jwtToken.Payload;
    }
}