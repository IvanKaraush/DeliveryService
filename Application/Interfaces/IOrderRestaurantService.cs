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
    public interface IOrderRestaurantService
    {
        Task<List<OrderModel>> GetOrdersList(int count, Coordinates restaurantCoordinates);
        Task AcceptOrder(Guid id);
        Task<OrderModel> GetOrderById(Guid id);
        Task RemoveUnitFromList(Guid id, int article, bool wasCookedEarlier);
    }
}
