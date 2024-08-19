using Domain.Models.Entities.MongoDBEntities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IMongoContext
    {
        public IMongoCollection<SoldProduct> SoldGoods { get; }
        public IMongoCollection<Report> Reports { get; }
        public IMongoCollection<Order> Orders { get; }
        public IMongoCollection<AuditLogRecord> AuditRecords { get; }
    }
}
