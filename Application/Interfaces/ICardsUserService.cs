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
        Task AddCard(CardModel card, Guid userId);
        Task RemoveCard(string number);
        Task<List<Card>> UserCards(Guid userId);
        Task<Card> GetCardByNumber(string number);
    }
}
