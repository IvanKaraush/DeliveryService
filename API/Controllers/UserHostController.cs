using Application.Interfaces;
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
            UserHostService = userHostService;
        }
        private readonly IUserHostService UserHostService;
        [HttpPatch]
        [Route("debitbonuses")]
        public async Task<IActionResult> DebitBonuses(Guid id, decimal amount)
        {
            await UserHostService.DebitBonuses(id, amount);
            return Ok();
        }
        [HttpPatch]
        [Route("assignadm")]
        public async Task<IActionResult> AssignAdm(Guid id)
        {
            await UserHostService.AssignAsAdmin(id);
            return Ok();
        }
        [HttpPatch]
        [Route("unassignadm")]
        public async Task<IActionResult> UnassignAdm(Guid id)
        {
            await UserHostService.UnassignAsAdmin(id);
            return Ok();
        }
    }
}
