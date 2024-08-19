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
            _reportStore = reportStore;
            _auditLogStore = auditLogStore;
        }
        private readonly IReportStore _reportStore;
        private readonly IAuditLogStore _auditLogStore;
        public async Task<Report> GetReportById(DateTime id)
        {
            return await _reportStore.GetReportById(id);
        }

        public async Task<List<DateTime>> GetReportsIds()
        {
            return await _reportStore.GetReportsIds();
        }

        public async Task RemoveReport(DateTime id, Guid admin)
        {
            await _reportStore.RemoveReport(id);
            await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.REPORT_CLOSED}{id.ToString()}"));
        }
    }
}
