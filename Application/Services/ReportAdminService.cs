using Application.Interfaces;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReportAdminService : IReportAdminService
    {
        public ReportAdminService(IReportStore reportStore, IAuditLogStore auditLogStore) 
        {
            ReportStore = reportStore;
            AuditLogStore = auditLogStore;
        }
        private readonly IReportStore ReportStore;
        private readonly IAuditLogStore AuditLogStore;
        public async Task<Report> GetReportById(DateTime id)
        {
            return await ReportStore.GetReportById(id);
        }

        public async Task<List<DateTime>> GetReportsIds()
        {
            return await ReportStore.GetReportsIds();
        }

        public async Task RemoveReport(DateTime id, Guid admin)
        {
            await ReportStore.RemoveReport(id);
            await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Закрыто обращение " + id.ToString()));
        }
    }
}
