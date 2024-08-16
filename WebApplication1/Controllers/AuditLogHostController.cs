using Application.Interfaces;
using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("auditlog")]
    [Authorize("RequireHostRole")]
    public class AuditLogHostController : ControllerBase
    {
        public AuditLogHostController(IAuditHostService auditHostService)
        {
            AuditHostService = auditHostService;
        }
        private readonly IAuditHostService AuditHostService;
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> Count()
        {
            return Ok(await AuditHostService.GetRecordsCount());
        }
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> list(int count)
        {
            return Ok(await AuditHostService.GetLastRecords(count));
        }
    }
}
