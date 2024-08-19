using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.VievModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderRestaurantService
    {
        public Task<OrderModel> GetOrderById(Guid id);
        public Task<List<OrderModel>> GetOrdersList(int count, Coordinates restaurantCoordinates);
        public Task AcceptOrder(Guid id);
        public Task RemoveUnitFromList(Guid id, int article, bool wasCookedEarlier);
        public Task RemoveOrder(Guid id);
    }
}
