using Domain.Models.Entities;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.VievModels;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Stores
{
    public interface IOrderStore
    {
        public Task<Order> GetOrderById(Guid id);
        public Task<List<Order>> GetOrdersByUserId(Guid id);
        public Task<List<Order>> GetOrdersList(int count, Coordinates restaurantCoordinates);
        public Task AddOrder(Order order);
        public Task RemoveOrder(Guid id);
        public Task AcceptOrder(Guid id);
        public Task<DateTime> RemoveUnitFromList(Guid id, int article);
    }
}
