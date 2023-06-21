using JwtAuthDemo.Infrastructure;
using JwtAuthDemo.Models;
using JwtAuthDemo.Models.Reponses;
using JwtAuthDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtAuthDemo.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private DBContext _dbContext;
        public TokenController(ILogger<TokenController> logger, IUserService userService, IJwtAuthManager jwtAuthManager, DBContext dBContext)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _dbContext = dBContext;
        }

        [HttpPost("addToken")]
        [Authorize]
        public async Task<string> addToken(string name, int numDateExpired)
        {
            var userName = User.Identity?.Name;
            var role = _userService.GetUserRole(userName);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Role, role)
            };
            var jwtResult = _jwtAuthManager.GenerateTokensCustom(userName, claims, DateTime.Now, numDateExpired);
            Token token = new Token();
            token.ID = Guid.NewGuid();
            token.Name = name;
            token.TokenString = jwtResult.AccessToken;
            token.NumberDateExpired = numDateExpired;
            token.UserID = _userService.GetUserID(userName);
            _dbContext.Tokens.Add(token);
            _dbContext.SaveChanges();
            return jwtResult.AccessToken;
        }
        [HttpGet("getTokens")]
        [Authorize]
        public TokenResponse getTokens(int pageIndex, int pageSize)
        {
            var userName = User.Identity?.Name;
            var userID = _userService.GetUserID(userName);
            TokenResponse tokenResponse = new TokenResponse();
            tokenResponse.total = _dbContext.Tokens.Count();
            tokenResponse.items = _dbContext.Tokens.Where(t => t.UserID == userID).Skip(pageIndex * pageSize).Take(pageSize).ToArray();
            return tokenResponse;
        }

    }
}
