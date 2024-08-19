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
            _orderStore = orderStore;
            _soldProductStore = soldProductStore;
            _productStore = productStore;
        }
        private readonly IOrderStore _orderStore;
        private readonly ISoldProductStore _soldProductStore;
        private readonly IProductStore _productStore;
        public async Task<OrderModel> GetOrderById(Guid id)
        {
            return new OrderModel(await _orderStore.GetOrderById(id));
        }

        public async Task<List<OrderModel>> GetOrdersList(int count, Coordinates restaurantCoordinates)
        {
            List<Order> orders = await _orderStore.GetOrdersList(count, restaurantCoordinates);
            List<OrderModel> orderModels = new List<OrderModel>();
            foreach (Order order in orders)
            {
                orderModels.Add(new OrderModel(order));
            }
            return orderModels;
        }
        public async Task AcceptOrder(Guid id)
        {
            await _orderStore.AcceptOrder(id);
        }
        public async Task RemoveOrder(Guid id)
        {
            await _orderStore.RemoveOrder(id);
        }

        public async Task RemoveUnitFromList(Guid id, int article, bool wasCookedEarlier)
        {
            DateTime timeMarker = await _orderStore.RemoveUnitFromList(id, article);
            if (!wasCookedEarlier)
            {
                await _soldProductStore.AddSoldProduct(new SoldProduct(article));
                TimeOnly cookingTime = new TimeOnly((DateTime.Now - timeMarker).Ticks);
                await _productStore.UpdateCookingTime(article, cookingTime);
            }            
        }
    }
}
