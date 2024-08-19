using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICardsUserService
    {
        public Task<CardModel> GetCardByNumber(string number);
        public Task<List<CardModel>> GetUserCards(Guid userId);
        public Task AddCard(CardModel card, Guid userId);
        public Task RemoveCard(string number);
    }
}
