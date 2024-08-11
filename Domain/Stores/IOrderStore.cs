using Domain.Models.ApplicationModels;
using Domain.Models.Entities;
using Domain.Models.Entities.MongoDBEntities;
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
        public void AddOrder(Order order);
        public void RemoveOrder(Guid id);
        public Task<List<Order>> GetOrdersList(int count, Coordinates restaurantCoordinates);
        public void AcceptOrder(Guid id);
        public Task<Order> GetOrderById(Guid id);
        public Task<Order> GetOrderByUserId(Guid id);
        public void RemoveUnitFromList(Guid id, int article);
    }
}
