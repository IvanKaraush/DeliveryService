using Domain.Models.Entities.MongoDBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Stores
{
    public interface IReportStore
    {
        public void AddReport(Report report);
        public void RemoveReport(DateTime id);
        public Task<List<DateTime>> GetReportsIds();
        public Task<Report> GetReportById(DateTime id);
    }
}
