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
        Task AddOrder(OrderModel order);
        Task RemoveOrder(Guid id);
        Task<OrderModel> GetOrderById(Guid id);
        Task<List<OrderModel>> GetOrdersByUserId(Guid id);
    }
}
