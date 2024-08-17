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
            ReportAdminService = reportAdminService;
        }
        private readonly IReportAdminService ReportAdminService;
        [HttpDelete]
        [Route("close")]
        public async Task<IActionResult> Close(DateTime id)
        {
            await ReportAdminService.RemoveReport(id, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
        [HttpGet]
        [Route("reportkeys")]
        public async Task<IActionResult> ReportKeys()
        {
            return Ok(await ReportAdminService.GetReportsIds());
        }
        [HttpGet]
        [Route("report")]
        public async Task<IActionResult> Report(DateTime id)
        {
            return Ok(await ReportAdminService.GetReportById(id));
        }
    }
}
