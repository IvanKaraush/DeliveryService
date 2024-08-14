using Application.Interfaces;
using Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserHostService : IUserHostService
    {
        public UserHostService(IUserStore userStore)
        {
            UserStore = userStore;
        }
        private readonly IUserStore UserStore;
        public async Task AssignAsAdmin(Guid id)
        {
            await UserStore.AssignAsAdmin(id);
        }

        public async Task DebitBonuses(Guid id, decimal amount)
        {
            await UserStore.DebitBonuses(id, amount);
        }

        public async Task UnassignAsAdmin(Guid id)
        {
            await UserStore.UnassignAsAdmin(id);
        }
    }
}
