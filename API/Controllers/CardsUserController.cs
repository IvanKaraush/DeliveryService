using Application.Interfaces;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("cards")]
    [Authorize("RequireUserRole")]
    public class CardsUserController : ControllerBase
    {
        public CardsUserController(ICardsUserService cardsUserService) 
        {
            _cardsUserService = cardsUserService;
        }
        private readonly ICardsUserService _cardsUserService;

        [Route("getcard")]
        [HttpGet]
        public async Task<IActionResult> GetCard(string number)
        {
            if (number == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            var cards = await _cardsUserService.GetUserCards(userGuid);
            if (cards.Where(c => c.Number == number).Count() != 0)
                return Ok(await _cardsUserService.GetCardByNumber(number));
            return Forbid("You don\'t have a card with that number");
        }
        [Route("getlist")]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            return Ok(await _cardsUserService.GetUserCards(userGuid));
        }
        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> Add(CardModel card)
        {
            if (card == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            await _cardsUserService.AddCard(card, userGuid);
            return Ok();
        }
        [Route("remove")]
        [HttpDelete]
        public async Task<IActionResult> Remove(string number)
        {
            if (number == null)
                return BadRequest("Arguments are null");
            string? userGuidString = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (userGuidString == null)
                return BadRequest("User id in token not found");
            if (!Guid.TryParse(userGuidString, out Guid userGuid))
                return BadRequest("Invalid user id in token");
            var cards = await _cardsUserService.GetUserCards(userGuid);
            if (cards.Where(c => c.Number == number).Count() != 0)
            {
                await _cardsUserService.RemoveCard(number);
                return Ok();
            }
            return Forbid("You don\'t have a card with that number");
        }
    }
}
