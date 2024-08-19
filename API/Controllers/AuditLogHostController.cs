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
            _auditHostService = auditHostService;
        }
        private readonly IAuditHostService _auditHostService;
        [HttpGet]
        [Route("getcount")]
        public async Task<IActionResult> GetCount()
        {
            return Ok(await _auditHostService.GetRecordsCount());
        }
        [HttpGet]
        [Route("getlist")]
        public async Task<IActionResult> Getlist(int count)
        {
            return Ok(await _auditHostService.GetLastRecords(count));
        }
    }
}
