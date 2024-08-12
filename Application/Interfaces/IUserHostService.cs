using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserHostService
    {
        Task DebitBonuses(Guid id, decimal amount);
        Task AssignAsAdmin(Guid id);
        Task UnassignAsAdmin(Guid id);
    }
}
