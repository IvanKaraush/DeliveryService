using Application.Interfaces;
using Domain.Models.ApplicationModels;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Persistence;
using StackExchange.Redis;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        public AuthController(IAuthService authService, ITelegramBotApi telegramBotApi)
        {
            AuthService = authService;
            TelegramBotApi = telegramBotApi;
        }
        private readonly IAuthService AuthService;
        private readonly ITelegramBotApi TelegramBotApi;
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRegisterModel userRegisterModel)
        {
            await AuthService.RegisterUser(userRegisterModel);
            return Ok();
        }
        [Route("host")]
        [HttpPost]
        public async Task<IActionResult> AuthHost(AuthModel authModel)
        {
            if (!TelegramBotApi.IsHostLogined)
            {
                await TelegramBotApi.SendHostAuthMessage();
                return Unauthorized(); 
            }
            await AuthService.AuthHost(authModel);
            string role = "HostRole";
            DateTime utcNow = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.Issuer,
                    audience: AuthOptions.Client,
                    notBefore: utcNow,
                    claims: GetIdentity("Host", role).Claims,
                    expires: utcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(encodedJwt);
        }
        [Route("restaurant")]
        [HttpPost]
        public async Task<IActionResult> AuthRestaurant(AuthModel authModel)
        {
            Restaurant restaurant = await AuthService.GetRestaurantByAuth(authModel);
            string role = "RestaurantRole";
            DateTime utcNow = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.Issuer,
                    audience: AuthOptions.Client,
                    notBefore: utcNow,
                    claims: GetIdentity(restaurant.Adress, role).Claims,
                    expires: utcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(encodedJwt);
        }
        [Route("user")]
        [HttpPost]
        public async Task<IActionResult> AuthUser(AuthModel authModel)
        {
            UserOutputModel user = await AuthService.GetUserByAuth(authModel);
            string role = "UserRole";
            if (user.IsAdmin)
            {
                role = "AdminRole";
            }
            DateTime utcNow = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.Issuer,
                    audience: AuthOptions.Client,
                    notBefore: utcNow,
                    claims: GetIdentity(user.Id.ToString(), role).Claims,
                    expires: utcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(encodedJwt);
        }
        private ClaimsIdentity GetIdentity(string id, string role)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, id),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
