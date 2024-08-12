using Domain.Models.Entities.MongoDBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReportAdminService
    {
        Task RemoveReport(DateTime id);
        Task<List<DateTime>> GetReportsIds();
        Task<Report> GetReportById(DateTime id);
    }
}
