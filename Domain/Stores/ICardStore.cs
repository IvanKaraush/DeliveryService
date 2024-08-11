using Domain.Models.Entities;

namespace Domain.Stores
{
    public interface ICardStore
    {
        void AddCard(Card card);
        void RemoveCard(string number);
        Task<List<Card>> UserCards(Guid userId);
        Task<Card> GetCardByNumber(string number);
    }
}
