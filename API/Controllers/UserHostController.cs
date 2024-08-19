using Application.Interfaces;
using Domain.Models.Entities.SQLEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("users")]
    [ApiController]
    [Authorize("RequireHostRole")]
    public class UserHostController : ControllerBase
    {
        public UserHostController(IUserHostService userHostService) 
        {
            _userHostService = userHostService;
        }
        private readonly IUserHostService _userHostService;
        [HttpPatch]
        [Route("debitbonuses")]
        public async Task<IActionResult> DebitBonuses(Guid id, decimal amount)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _userHostService.DebitBonuses(id, amount);
            return Ok();
        }
        [HttpPatch]
        [Route("assignadm")]
        public async Task<IActionResult> AssignAdm(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _userHostService.AssignUserAsAdmin(id);
            return Ok();
        }
        [HttpPatch]
        [Route("unassignadm")]
        public async Task<IActionResult> UnassignAdm(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Arguments are null");
            await _userHostService.UnassignUserAsAdmin(id);
            return Ok();
        }
    }
}
