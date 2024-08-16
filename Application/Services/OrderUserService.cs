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
            OrderStore = orderStore;
            UserStore = userStore;
            CardStore = cardStore;
        }
        private readonly IOrderStore OrderStore;
        private readonly IUserStore UserStore;
        private readonly ICardStore CardStore;
        public async Task AddOrder(OrderModel order)
        {
            User user = await UserStore.GetUserById(order.UserId);
            if ((await CardStore.UserCards(user.Id)).Where(c=>c.Number == order.PaymentCard).Count()==0)
                throw new InvalidCardNumberException();
            await OrderStore.AddOrder(order.ToOrder());
        }

        public async Task<OrderModel> GetOrderById(Guid id)
        {
            return new OrderModel(await OrderStore.GetOrderById(id));
        }

        public async Task<List<OrderModel>> GetOrdersByUserId(Guid id)
        {
            List<OrderModel> orderModels = new List<OrderModel>();
            List<Order> orders = await OrderStore.GetOrdersByUserId(id);
            foreach (Order order in orders)
                orderModels.Add(new OrderModel(order));
            return orderModels;
        }

        public async Task RemoveOrder(Guid id)
        {
            await OrderStore.RemoveOrder(id);
        }
    }
}
