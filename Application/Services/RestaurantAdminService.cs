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
            RestaurantStore = restaurantStore;
            AuditLogStore = auditLogStore;
        }
        private readonly IRestaurantStore RestaurantStore;
        private readonly IAuditLogStore AuditLogStore;
        public async Task AddRestaurant(Restaurant restaurant, Guid admin)
        {
            await RestaurantStore.AddRestaurant(restaurant);
            await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Добавлен ресторан по адресу: " + restaurant.Adress));
        }

        public async Task EditRestaurantAuth(string adress, AuthModel authModel, Guid admin)
        {
            await RestaurantStore.EditRestaurantAuth(adress, authModel);
            await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Изменены логин и/или пароль ресторана по адресу: " + adress));
        }

        public async Task RemoveRestaurant(string adress, Guid admin)
        {
            await RestaurantStore.RemoveRestaurant(adress);
            await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Удален ресторан по адресу: " + adress));
        }
    }
}
