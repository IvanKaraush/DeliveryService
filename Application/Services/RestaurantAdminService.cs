using Application.Interfaces;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RestaurantAdminService : IRestaurantAdminService
    {
        public RestaurantAdminService(IRestaurantStore restaurantStore,IAuditLogStore auditLogStore) 
        {
            _restaurantStore = restaurantStore;
            _auditLogStore = auditLogStore;
        }
        private readonly IRestaurantStore _restaurantStore;
        private readonly IAuditLogStore _auditLogStore;

        public async Task EditRestaurantAuth(string adress, AuthModel authModel, Guid admin)
        {
            await _restaurantStore.EditRestaurantAuth(adress, authModel);
            await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.RESTAURANT_AUTH_CHANGED}{adress}"));
        }
        public async Task AddRestaurant(Restaurant restaurant, Guid admin)
        {
            await _restaurantStore.AddRestaurant(restaurant);
            await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.RESTAURANT_ADDED}{restaurant.Adress}"));
        }

        public async Task RemoveRestaurant(string adress, Guid admin)
        {
            await _restaurantStore.RemoveRestaurant(adress);
            await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.RESTAURANT_REMOVED}{adress}"));
        }
    }
}
