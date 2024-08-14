using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Options;
using Domain.Models.ApplicationModels;

namespace Persistence.Repository
{
    public class AuditLogRepository : IAuditLogStore
    {
        public AuditLogRepository(IMongoContext context, IOptions<ReposOptions> options) 
        {
            Context = context;
            AuditExpirationDays = options.Value.AuditExpirationDays;
        }
        private readonly IMongoContext Context;
        private readonly int AuditExpirationDays;
        public async Task AddRecord(AuditLogRecord record)
        {
            record.Recorded = DateTime.Now;
            record.ExpireAt = record.Recorded.AddDays(AuditExpirationDays);
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
