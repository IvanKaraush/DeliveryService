using Domain.Models.Entities.SQLEntities;

namespace Domain.Stores
{
    public interface ICardStore
    {
        public Task<Card> GetCardByNumber(string number);
        public Task<List<Card>> GetUserCards(Guid userId);
        public Task AddCard(Card card);
        public Task RemoveCard(string number);
    }
}
