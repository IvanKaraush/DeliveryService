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
            CardsUserService = cardsUserService;
        }
        private readonly ICardsUserService CardsUserService;
        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> Add(CardModel card)
        {
            await CardsUserService.AddCard(card, Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            return Ok();
        }
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return Ok(await CardsUserService.UserCards(Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value)));
        }
        [Route("card")]
        [HttpGet]
        public async Task<IActionResult> Card(string number)
        {
            var cards = await CardsUserService.UserCards(Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            if(cards.Where(c=>c.Number==number).Count()!=0)
                return Ok(await CardsUserService.GetCardByNumber(number));
            return Forbid("You don\'t have a card with that number");
        }
        [Route("remove")]
        [HttpDelete]
        public async Task<IActionResult> Remove(string number)
        {
            var cards = await CardsUserService.UserCards(Guid.Parse(HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value));
            if (cards.Where(c => c.Number == number).Count() != 0)
            {
                await CardsUserService.RemoveCard(number);
                return Ok();
            }
            return Forbid("You don\'t have a card with that number");
        }
    }
}
