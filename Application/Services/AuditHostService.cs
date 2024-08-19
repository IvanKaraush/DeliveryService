using Application.Interfaces;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuditHostService : IAuditHostService
    {
        public AuditHostService(IAuditLogStore auditLogStore)
        {
            _auditLogStore = auditLogStore;
        }
        IAuditLogStore _auditLogStore;
        public async Task<List<AuditLogRecord>> GetLastRecords(int count)
        {
            return await _auditLogStore.GetLastRecords(count);
        }

        public async Task<int> GetRecordsCount()
        {
            return await _auditLogStore.GetRecordsCount();
        }
    }
}
