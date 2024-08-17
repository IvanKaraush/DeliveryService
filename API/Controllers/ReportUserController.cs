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
            ReportUserService = reportUserService;
        }
        private readonly IReportUserService ReportUserService;
        [HttpPost]
        [Route("leavereport")]
        [Authorize("RequireUserRole")]
        public async Task<IActionResult> LeaveReport(string message)
        {
            await ReportUserService.AddReport(message, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
    }
}
