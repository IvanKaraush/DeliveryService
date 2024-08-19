using Domain.Models.Entities.SQLEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUserCacheService
    {
        public Task<User?> Get(Guid id);
        public Task Save(User user);
        public Task Remove(Guid id);
        public Task Update(User user);
    }
}
