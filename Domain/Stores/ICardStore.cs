using Domain.Models.Entities.SQLEntities;

namespace Domain.Stores
{
    public interface ICardStore
    {
        Task AddCard(Card card);
        Task RemoveCard(string number);
        Task<List<Card>> UserCards(Guid userId);
        Task<Card> GetCardByNumber(string number);
    }
}
