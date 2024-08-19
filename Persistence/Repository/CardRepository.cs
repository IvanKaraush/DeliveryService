using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Stores;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Persistence.Exceptions;
using System.Security.Cryptography.Pkcs;

namespace Persistence.Repository
{
    public class CardRepository : ICardStore
    {
        public CardRepository(SQLContext context)
        {
            _context = context;
        }
        private readonly SQLContext _context;

        public async Task<Card> GetCardByNumber(string number)
        {
            Card? card = await _context.Cards.FirstOrDefaultAsync(u => u.Number.Equals(number));
            if (card == null)
                throw new DoesNotExistException(typeof(Card));
            return card;
        }

        public async Task<List<Card>> GetUserCards(Guid userId)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            return await _context.Cards.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task AddCard(Card card)
        {
            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCard(string number)
        {
            Card? card = await _context.Cards.FirstOrDefaultAsync(u => u.Number.Equals(number));
            if (card == null)
                throw new DoesNotExistException(typeof(Card));
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
        }
    }
}
