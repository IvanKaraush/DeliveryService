﻿using Application.Interfaces;
using Domain.Models.ApplicationModels;
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
            CardStore = cardStore;
        }
        ICardStore CardStore;
        public async Task AddCard(CardModel cardModel, Guid userId)
        {
            if (!long.TryParse(cardModel.Number, out long r))
                throw new IncorrectCardNumberException();
            await CardStore.AddCard(cardModel.ToCard(userId));
        }

        public async Task<Card> GetCardByNumber(string number)
        {
            if (!long.TryParse(number, out long r))
                throw new IncorrectCardNumberException();
            return await CardStore.GetCardByNumber(number);
        }

        public async Task RemoveCard(string number)
        {
            if (!long.TryParse(number, out long r))
                throw new IncorrectCardNumberException();
            await CardStore.RemoveCard(number);
        }

        public async Task<List<CardModel>> UserCards(Guid userId)
        {
            List<Card> cardList = await CardStore.UserCards(userId);
            List<CardModel> result = new();
            foreach (Card card in cardList)
            {
                result.Add(new CardModel(card));
            }
            return result;
        }
    }
}
