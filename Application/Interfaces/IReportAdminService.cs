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
        public Task<Report> GetReportById(DateTime id);
        public Task<List<DateTime>> GetReportsIds();
        public Task RemoveReport(DateTime id, Guid admin);
    }
}
