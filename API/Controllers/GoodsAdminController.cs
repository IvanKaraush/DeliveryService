using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("goods")]
    [ApiController]
    [Authorize("RequireAdminRole")]
    public class GoodsAdminController : ControllerBase
    {
        public GoodsAdminController(IGoodsAdminService goodsAdminService) 
        {
            GoodsAdminService = goodsAdminService;
        }
        private readonly IGoodsAdminService GoodsAdminService;

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(ProductInputModel product)
        {
            await GoodsAdminService.AddProduct(product, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
        [HttpDelete]
        [Route("remove")]
        public async Task<IActionResult> Remove(int article)
        {
            await GoodsAdminService.RemoveProduct(article, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
        [HttpPatch]
        [Route("editprice")]
        public async Task<IActionResult> EditPrice(int article, decimal price)
        {
            await GoodsAdminService.EditPrice(article, price, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
        [HttpPatch]
        [Route("show")]
        public async Task<IActionResult> Show(int article)
        {
            await GoodsAdminService.Show(article, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
        [HttpPatch]
        [Route("hide")]
        public async Task<IActionResult> Hide(int article)
        {
            await GoodsAdminService.Hide(article, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
        [HttpGet]
        [Route("invisiblelist")]
        public async Task<IActionResult> InvisibleList(int page, int pageSize, string? textInTitle)
        {
            return Ok(await GoodsAdminService.GetInvisibleGoodsList(page, pageSize, textInTitle));
        }
        [HttpPost]
        [Route("attachimage")]
        public async Task<IActionResult> AttachImage(IFormFile file, int article)
        {
            await GoodsAdminService.AttachImage(file, article, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
        [HttpDelete]
        [Route("detachimage")]
        public async Task<IActionResult> DetachImage(int article)
        {
            await GoodsAdminService.DetachImage(article, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
    }
}
