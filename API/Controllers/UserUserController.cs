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
            _userUserService = userUserService;
        }
        private readonly IUserUserService _userUserService;
        [HttpGet]
        [Route("getuser")]
        public async Task<IActionResult> GetUser()
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            return Ok(await _userUserService.GetUserById(userGuid));
        }
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete()
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _userUserService.DeleteUser(userGuid);
            return Ok();
        }
        [HttpPatch]
        [Route("edittg")]
        public async Task<IActionResult> EditTG(string telegram)
        {
            if (telegram == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _userUserService.EditUserTelegram(userGuid, telegram);
            return Ok();
        }
        [HttpPatch]
        [Route("addbirthdate")]
        public async Task<IActionResult> AddBirthDate(DateOnly birthDate)
        {
            if (birthDate == DateOnly.MinValue)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _userUserService.AddUserBirthDate(userGuid, birthDate);
            return Ok();
        }
        [HttpPatch]
        [Route("editauth")]
        public async Task<IActionResult> EditAuth(AuthModel authModel)
        {
            if (authModel == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _userUserService.EditUserAuth(userGuid, authModel);
            return Ok();
        }
    }
}
