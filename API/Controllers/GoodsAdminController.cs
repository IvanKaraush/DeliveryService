using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("goods")]
    [ApiController]
    [Authorize("RequireAdminRole")]
    public class GoodsAdminController : ControllerBase
    {
        public GoodsAdminController(IGoodsAdminService goodsAdminService) 
        {
            _goodsAdminService = goodsAdminService;
        }
        private readonly IGoodsAdminService _goodsAdminService;

        [HttpGet]
        [Route("invisiblelist")]
        public async Task<IActionResult> InvisibleList(int page, int pageSize, string? textInTitle)
        {
            return Ok(await _goodsAdminService.GetInvisibleGoodsList(page, pageSize, textInTitle));
        }
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(ProductInputModel product)
        {
            if (product == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.AddProduct(product, userGuid);
            return Ok();
        }
        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> Remove(int article)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.RemoveProduct(article, userGuid);
            return Ok();
        }
        [HttpPatch]
        [Route("editprice")]
        public async Task<IActionResult> EditPrice(int article, decimal price)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.EditPrice(article, price, userGuid);
            return Ok();
        }
        [HttpPatch]
        [Route("show")]
        public async Task<IActionResult> Show(int article)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.ShowProduct(article, userGuid);
            return Ok();
        }
        [HttpPatch]
        [Route("hide")]
        public async Task<IActionResult> Hide(int article)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.HideProduct(article, userGuid);
            return Ok();
        }
        [HttpPost]
        [Route("attachimage")]
        public async Task<IActionResult> AttachImage(IFormFile file, int article)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.AttachImage(file, article, userGuid);
            return Ok();
        }
        [HttpDelete]
        [Route("detachimage")]
        public async Task<IActionResult> DetachImage(int article)
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _goodsAdminService.DetachImage(article, userGuid);
            return Ok();
        }
    }
}
