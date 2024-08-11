using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Persistence.Repository
{
    public class AuditLogRepository : IAuditLogStore
    {
        public AuditLogRepository(IMongoContext context) 
        {
            Context = context;
        }
        private readonly IMongoContext Context;
        public async void AddRecord(AuditLogRecord record)
        {
            record.Recorded = DateTime.Now;
            await Context.AuditRecords.InsertOneAsync(record);
        }

        public async Task<List<AuditLogRecord>> GetLastRecords(int count)
        {
            return await Context.AuditRecords.AsQueryable().Take(count).ToListAsync();
        }

        public async Task<int> GetRecordsCount()
        {
            return await Context.AuditRecords.AsQueryable().CountAsync();
        }
    }
}
