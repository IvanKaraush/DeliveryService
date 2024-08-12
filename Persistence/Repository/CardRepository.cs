using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Security.Cryptography.Pkcs;

namespace Persistence.Repository
{
    public class CardRepository : ICardStore
    {
        public CardRepository(SQLContext context)
        {
            Context = context;
        }
        private readonly SQLContext Context;
        public async Task AddCard(Card card)
        {
            await Context.Cards.AddAsync(card);
            await Context.SaveChangesAsync();
        }

        public async Task RemoveCard(string number)
        {
            Card? card = await Context.Cards.FirstOrDefaultAsync(u => u.Number.Equals(number));
            if (card == null)
                throw new DoesNotExistException(typeof(Card));
            Context.Cards.Remove(card);
            await Context.SaveChangesAsync();
        }

        public async Task<List<Card>> UserCards(Guid userId)
        {
            User? user = await Context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            return await Context.Cards.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<Card> GetCardByNumber(string number)
        {
            Card? card = await Context.Cards.FirstOrDefaultAsync(u => u.Number.Equals(number));
            if (card == null)
                throw new DoesNotExistException(typeof(Card));
            return card;
        }
    }
}
