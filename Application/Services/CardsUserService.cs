using Application.Interfaces;
using Domain.Models.ApplicationModels;
using Domain.Models.ApplicationModels.Exceptions;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CardsUserService : ICardsUserService
    {
        public CardsUserService(ICardStore cardStore)
        {
            _cardStore = cardStore;
        }
        ICardStore _cardStore;

        public async Task<CardModel> GetCardByNumber(string number)
        {
            if (!ulong.TryParse(number, out ulong r) || number.Length != 16)
                throw new InvalidCardNumberException();
            return new CardModel(await _cardStore.GetCardByNumber(number));
        }
        public async Task<List<CardModel>> GetUserCards(Guid userId)
        {
            List<Card> cardList = await _cardStore.GetUserCards(userId);
            List<CardModel> result = new();
            foreach (Card card in cardList)
            {
                result.Add(new CardModel(card));
            }
            return result;
        }
        public async Task AddCard(CardModel cardModel, Guid userId)
        {
            await _cardStore.AddCard(cardModel.ToCard(userId));
        }
        public async Task RemoveCard(string number)
        {
            if (!ulong.TryParse(number, out ulong r)||number.Length!=16)
                throw new InvalidCardNumberException();
            await _cardStore.RemoveCard(number);
        }
    }
}
