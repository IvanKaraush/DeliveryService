using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("reports")]
    [ApiController]
    [Authorize("RequireAdminRole")]
    public class ReportAdminController : ControllerBase
    {
        public ReportAdminController(IReportAdminService reportAdminService)
        {
            _reportAdminService = reportAdminService;
        }
        private readonly IReportAdminService _reportAdminService;

        [HttpGet]
        [Route("getreport")]
        public async Task<IActionResult> GetReport(DateTime id)
        {
            if (id == DateTime.MinValue)
                return BadRequest("Arguments are null");
            return Ok(await _reportAdminService.GetReportById(id));
        }
        [HttpGet]
        [Route("getreportkeys")]
        public async Task<IActionResult> GetReportKeys()
        {
            return Ok(await _reportAdminService.GetReportsIds());
        }
        [HttpDelete]
        [Route("closereport")]
        public async Task<IActionResult> CloseReport(DateTime id)
        {
            if (id == DateTime.MinValue)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _reportAdminService.RemoveReport(id, userGuid);
            return Ok();
        }
    }
}
