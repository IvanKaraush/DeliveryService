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
        Task AddOrder(Order order);
        Task RemoveOrder(Guid id);
        Task<List<Order>> GetOrdersList(int count, Coordinates restaurantCoordinates);
        Task AcceptOrder(Guid id);
        Task<Order> GetOrderById(Guid id);
        Task<List<Order>> GetOrdersByUserId(Guid id);
        Task RemoveUnitFromList(Guid id, int article);
    }
}
