using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("goods")]
    [ApiController]
    public class GoodsUserController : ControllerBase
    {
        public GoodsUserController(IGoodsUserService goodsUserService) 
        {
            _goodsUserService = goodsUserService;
        }
        private readonly IGoodsUserService _goodsUserService;

        [HttpGet]
        [Route("getproduct")]
        public async Task<IActionResult> GetProduct(int article)
        {
            return Ok(await _goodsUserService.GetProduct(article));
        }
        [HttpGet]
        [Route("gethotlist")]
        public async Task<IActionResult> GetHotList(int count)
        {
            return Ok(await _goodsUserService.GetHotGoodsList(count));
        }
        [HttpPost]
        [Route("getlist")]
        public async Task<IActionResult> GetList(int page, int pageSize, GoodsListOptionsModel listOptions)
        {
            if (listOptions == null)
                return BadRequest("Arguments are null");
            return Ok(await _goodsUserService.GetVisibleGoodsList(page, pageSize, listOptions));
        }
        [HttpPatch]
        [Route("rate")]
        [Authorize("RequireUserRole")]
        public async Task<IActionResult> Rate(int article, int mark)
        {
            await _goodsUserService.RateProduct(article, mark);
            return Ok();
        }
    }
}
