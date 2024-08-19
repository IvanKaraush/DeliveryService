using Domain.Models.Entities.MongoDBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Stores
{
    public interface IAuditLogStore
    {
        public Task<List<AuditLogRecord>> GetLastRecords(int count);
        public Task<int> GetRecordsCount();
        public Task AddRecord(AuditLogRecord record);
    }
}
