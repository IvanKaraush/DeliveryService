using Application.Interfaces;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderRestaurantService : IOrderRestaurantService
    {
        public OrderRestaurantService(IOrderStore orderStore, ISoldProductStore soldProductStore, IProductStore productStore)
        {
            OrderStore = orderStore;
            SoldProductStore = soldProductStore;
            ProductStore = productStore;
        }
        private readonly IOrderStore OrderStore;
        private readonly ISoldProductStore SoldProductStore;
        private readonly IProductStore ProductStore;
        public async Task AcceptOrder(Guid id)
        {
            await OrderStore.AcceptOrder(id);
        }

        public async Task<OrderModel> GetOrderById(Guid id)
        {
            return new OrderModel(await OrderStore.GetOrderById(id));
        }

        public async Task<List<OrderModel>> GetOrdersList(int count, Coordinates restaurantCoordinates)
        {
            List<Order> orders = await OrderStore.GetOrdersList(count, restaurantCoordinates);
            List<OrderModel> orderModels = new List<OrderModel>();
            foreach (Order order in orders)
            {
                orderModels.Add(new OrderModel(order));
            }
            return orderModels;
        }

        public async Task RemoveUnitFromList(Guid id, int article, bool wasCookedEarlier)
        {
            DateTime timeMarker = await OrderStore.RemoveUnitFromList(id, article);
            if (!wasCookedEarlier)
            {
                await SoldProductStore.AddSoldProduct(new SoldProduct(article));
                TimeOnly cookingTime = new TimeOnly((DateTime.Now - timeMarker).Ticks);
                await ProductStore.UpdateCookingTime(article, cookingTime);
            }            
        }
    }
}
