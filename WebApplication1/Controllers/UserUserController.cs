using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("users")]
    [ApiController]
    [Authorize("RequireUserRole")]
    public class UserUserController : ControllerBase
    {
        public UserUserController(IUserUserService userUserService) 
        {
            UserUserService = userUserService;
        }
        private readonly IUserUserService UserUserService;
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete()
        {
            await UserUserService.DeleteUser(Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
        [HttpPatch]
        [Route("edittg")]
        public async Task<IActionResult> EditTG(string telegram)
        {
            await UserUserService.EditUserTelegram(Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value), telegram);
            return Ok();
        }
        [HttpPatch]
        [Route("addbirthdate")]
        public async Task<IActionResult> AddBirthDate(DateOnly birthDate)
        {
            await UserUserService.AddUserBirthDate(Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value), birthDate);
            return Ok();
        }
        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> User()
        {
            return Ok(await UserUserService.GetUserById(Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value)));
        }
        [HttpPatch]
        [Route("editauth")]
        public async Task<IActionResult> EditAuth(AuthModel authModel)
        {
            await UserUserService.EditUserAuth(Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value), authModel);
            return Ok();
        }
    }
}
