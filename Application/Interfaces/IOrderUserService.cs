using Domain.Models.ApplicationModels;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.VievModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderUserService
    {
        public Task<OrderModel> GetOrderById(Guid id);
        public Task<List<OrderModel>> GetOrdersByUserId(Guid id);
        public Task AddOrder(OrderModel order);
        public Task RemoveOrder(Guid id);
    }
}
