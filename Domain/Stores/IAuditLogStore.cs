﻿using Domain.Models.Entities.MongoDBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Stores
{
    public interface IAuditLogStore
    {
        Task AddRecord(AuditLogRecord record);
        Task<List<AuditLogRecord>> GetLastRecords(int count);
        Task<int> GetRecordsCount();
    }
}
