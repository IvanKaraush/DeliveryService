using Application.Interfaces;
using Domain.Models.VievModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderUserService : IOrderUserService
    {
        public Task AddOrder(OrderModel order)
        {
            throw new NotImplementedException();
        }

        public Task<OrderModel> GetOrderById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderModel>> GetOrdersByUserId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveOrder(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
