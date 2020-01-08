using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SampleAngular.Api.Identity;
using SampleAngular.Core.Services;
using SampleAngular.Dto;

namespace SampleAngular.Api.Controllers
{
    [Route("Token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TokenController(IUserService userService, IConfiguration configuration, 
            ILogger<TokenController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Request a user token
        /// </summary>
        /// <param name="dto">Request token body</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RequestToken(RequestTokenDto dto)
        {
            _logger.LogRequest("Requesting user token for user {UserName}", dto?.UserName);

            var signInResult = await _userService.ValidateUserPassword(dto.UserName, dto.Password);
            if (!signInResult)
            {
                _logger.LogWarning("Username or password is invalid. Username: {UserName}", dto?.UserName);
                return BadRequest("Username or password is invalid");
            }

            var user = await _userService.GetUser(dto.UserName);
            if (!user.IsActive)
            {
                _logger.LogWarning("User is suspended");
                return BadRequest("User is suspended");
            }

            var tokenKey = _configuration["Security:Tokens:Key"];
            var tokenIssuer = _configuration["Security:Tokens:Issuer"];
            var tokenAudience = _configuration["Security:Tokens:Audience"];

            var token = AuthorizationToken.GenerateToken(user.Id, user.UserName, user.FirstName, user.LastName,
                user.Role, tokenKey, tokenIssuer, tokenAudience);

            _logger.LogResponse("Token for user {UserName} retrieved", dto?.UserName);

            return Ok(token);
        }

        /// <summary>
        /// Refresh a user token
        /// </summary>
        /// <returns></returns>
        [HttpGet("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var userId = User.GetUserId();
            _logger.LogRequest("Refreshing user token for user {userId}", userId);

            var user = await _userService.GetUserById(userId);
            if (!user.IsActive)
            {
                _logger.LogWarning("User is suspended");
                return BadRequest("User is suspended");
            }

            var tokenKey = _configuration["Security:Tokens:Key"];
            var tokenIssuer = _configuration["Security:Tokens:Issuer"];
            var tokenAudience = _configuration["Security:Tokens:Audience"];

            var token = AuthorizationToken.GenerateToken(user.Id, user.UserName, user.FirstName, user.LastName,
                user.Role, tokenKey, tokenIssuer, tokenAudience);

            _logger.LogRequest("Refreshed token for user {userId} retrieved", userId);

            return Ok(token);
        }
    }
}