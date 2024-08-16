using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("goods")]
    [ApiController]
    public class GoodsUserController : ControllerBase
    {
        public GoodsUserController(IGoodsUserService goodsUserService) 
        {
            GoodsUserService = goodsUserService;
        }
        private readonly IGoodsUserService GoodsUserService;
        [HttpGet]
        [Route("hot")]
        public async Task<IActionResult> Hot(int count)
        {
            return Ok(await GoodsUserService.GetHotGoodsList(count));
        }
        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> List(int page, int pageSize, GoodsListOptionsModel listOptions)
        {
            return Ok(await GoodsUserService.GetVisibleGoodsList(page, pageSize, listOptions));
        }
        [HttpGet]
        [Route("product")]
        public async Task<IActionResult> Product(int article)
        {
            return Ok(await GoodsUserService.GetProduct(article));
        }
        [HttpPatch]
        [Route("rate")]
        [Authorize("RequireUserRole")]
        public async Task<IActionResult> Rate(int article, int mark)
        {
            await GoodsUserService.RateProduct(article, mark);
            return Ok();
        }
    }
}
