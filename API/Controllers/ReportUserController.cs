using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("reports")]
    [ApiController]
    public class ReportUserController : ControllerBase
    {
        public ReportUserController(IReportUserService reportUserService) 
        {
            _reportUserService = reportUserService;
        }
        private readonly IReportUserService _reportUserService;
        [HttpPost]
        [Route("leavereport")]
        [Authorize("RequireUserRole")]
        public async Task<IActionResult> LeaveReport(string message)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _reportUserService.AddReport(message, userGuid);
            return Ok();
        }
    }
}
