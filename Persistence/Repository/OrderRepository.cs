using Domain.Models.ApplicationModels;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Persistence.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class OrderRepository : IOrderStore
    {
        public OrderRepository(IMongoContext context) 
        {
            _context = context;
        }
        private readonly IMongoContext _context;

        public async Task<Order> GetOrderById(Guid id)
        {
            Order? order = await _context.Orders.AsQueryable().FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                throw new DoesNotExistException(typeof(Order));
            return order;
        }

        public async Task<List<Order>> GetOrdersByUserId(Guid id)
        {
            List<Order> orders = await _context.Orders.AsQueryable().Where(o => o.UserId == id).ToListAsync();
            return orders;
        }

        public async Task<List<Order>> GetOrdersList(int count, Coordinates restaurantCoordinates)
        {
            return await _context.Orders.AsQueryable().OrderBy(o => o.Coordinates.CalcDistance(restaurantCoordinates)).Take(count).ToListAsync();
        }

        [Obsolete]
        public async Task AcceptOrder(Guid id)
        {
            Order? order = await _context.Orders.AsQueryable().Where(o => !o.IsCooking).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                throw new DoesNotExistException(typeof(Order));
            order.IsCooking = true;
            await _context.Orders.ReplaceOneAsync(new BsonDocument("Id", id), order);
        }

        public async Task AddOrder(Order order)
        {
            await _context.Orders.InsertOneAsync(order);
        }

        [Obsolete]
        public async Task RemoveOrder(Guid id)
        {
            await _context.Orders.DeleteOneAsync(new BsonDocument("Id", id));
        }

        [Obsolete]
        public async Task<DateTime> RemoveUnitFromList(Guid id, int article)
        {
            Order? order = await _context.Orders.AsQueryable().FirstOrDefaultAsync(o => o.Id == id && o.IsCooking);
            if (order == null)
                throw new DoesNotExistException(typeof(Order));
            var goodsList = order.GoodsList;
            if (!goodsList.TryGetValue(article, out int count))
                throw new DoesNotExistException(typeof(Product));
            if (count <= 1)
                goodsList.Remove(article);
            else
                goodsList[article]--;
            if (order.TimeMarker == null)
                throw new Exception("order.TimeMarker is null");
            DateTime oldTimeMarker = order.TimeMarker.Value;
            order.TimeMarker = DateTime.Now;
            await _context.Orders.ReplaceOneAsync(new BsonDocument("Id", id), order);
            return oldTimeMarker;
        }
    }
}
