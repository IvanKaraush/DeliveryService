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
            _userStore = userStore;
        }
        private readonly IUserStore _userStore;
        public async Task AssignUserAsAdmin(Guid id)
        {
            await _userStore.AssignAsAdmin(id);
        }

        public async Task DebitBonuses(Guid id, decimal amount)
        {
            await _userStore.DebitBonuses(id, amount);
        }

        public async Task UnassignUserAsAdmin(Guid id)
        {
            await _userStore.UnassignAsAdmin(id);
        }
    }
}
