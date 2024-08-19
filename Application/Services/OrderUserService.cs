using Application.Interfaces;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Entities.MongoDBEntities;

namespace Application.Services
{
    public class OrderUserService : IOrderUserService
    {
        public OrderUserService(IOrderStore orderStore, IUserStore userStore, ICardStore cardStore) 
        {
            _orderStore = orderStore;
            _userStore = userStore;
            _cardStore = cardStore;
        }
        private readonly IOrderStore _orderStore;
        private readonly IUserStore _userStore;
        private readonly ICardStore _cardStore;
        public async Task<OrderModel> GetOrderById(Guid id)
        {
            return new OrderModel(await _orderStore.GetOrderById(id));
        }
        public async Task<List<OrderModel>> GetOrdersByUserId(Guid id)
        {
            List<OrderModel> orderModels = new List<OrderModel>();
            List<Order> orders = await _orderStore.GetOrdersByUserId(id);
            foreach (Order order in orders)
                orderModels.Add(new OrderModel(order));
            return orderModels;
        }
        public async Task AddOrder(OrderModel order)
        {
            User user = await _userStore.GetUserById(order.UserId);
            if ((await _cardStore.GetUserCards(user.Id)).Where(c=>c.Number == order.PaymentCard).Count()==0)
                throw new InvalidCardNumberException();
            await _orderStore.AddOrder(order.ToOrder());
        }

        public async Task RemoveOrder(Guid id)
        {
            await _orderStore.RemoveOrder(id);
        }
    }
}
