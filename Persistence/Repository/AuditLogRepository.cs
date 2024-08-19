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
using Infrastructure.Interfaces;

namespace Persistence.Repository
{
    public class AuditLogRepository : IAuditLogStore
    {
        public AuditLogRepository(IMongoContext context, IOptions<RepositoryOptions> options) 
        {
            _ontext = context;
            _auditExpirationDays = options.Value.AuditExpirationDays;
        }
        private readonly IMongoContext _ontext;
        private readonly int _auditExpirationDays;

        public async Task<List<AuditLogRecord>> GetLastRecords(int count)
        {
            return await _ontext.AuditRecords.AsQueryable().Take(count).ToListAsync();
        }
        public async Task<int> GetRecordsCount()
        {
            return await _ontext.AuditRecords.AsQueryable().CountAsync();
        }
        public async Task AddRecord(AuditLogRecord record)
        {
            record.Recorded = DateTime.Now;
            record.ExpireAt = record.Recorded.AddDays(_auditExpirationDays);
            await _ontext.AuditRecords.InsertOneAsync(record);
        }
    }
}
