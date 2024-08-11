using Domain.Models.ApplicationModels;
using Domain.Models.Entities;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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
            Context = context;
        }
        private readonly IMongoContext Context;
        public async void AcceptOrder(Guid id)
        {
            Order? order = await Context.Orders.AsQueryable().Where(o => !o.IsCooking).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                throw new DoesNotExistException(typeof(Order));
            order.IsCooking = true;
            await Context.Orders.ReplaceOneAsync(new BsonDocument("Id", id), order);
        }

        public async void AddOrder(Order order)
        {
            await Context.Orders.InsertOneAsync(order);
        }

        public async Task<Order> GetOrderById(Guid id)
        {
            Order? order = await Context.Orders.AsQueryable().FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                throw new DoesNotExistException(typeof(Order));
            return order;
        }

        public async Task<List<Order>> GetOrdersList(int count, Coordinates restaurantCoordinates)
        {
            return await Context.Orders.AsQueryable().OrderBy(o=>o.Coordinates.CalcDistance(restaurantCoordinates)).Take(count).ToListAsync();
        }

        public async void RemoveOrder(Guid id)
        {
            await Context.Orders.DeleteOneAsync(new BsonDocument("Id", id));
        }

        public async void RemoveUnitFromList(Guid id, int article)
        {
            Order? order = await Context.Orders.AsQueryable().FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                throw new DoesNotExistException(typeof(Order));
            var goodsList = order.GoodsList;
            if (!goodsList.TryGetValue(article, out int count))
                throw new DoesNotExistException(typeof(Product));
            if (count <= 1)
                goodsList.Remove(article);
            else
                goodsList[article]--;
            order.TimeMarker = DateTime.Now;
            await Context.Orders.ReplaceOneAsync(new BsonDocument("Id", id), order);
        }

        public async Task<Order> GetOrderByUserId(Guid id)
        {
            Order? order = await Context.Orders.AsQueryable().FirstOrDefaultAsync(o => o.UserId == id);
            if (order == null)
                throw new DoesNotExistException(typeof(Order));
            return order;
        }
    }
}
