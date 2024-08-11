using Domain.Models.ApplicationModels;
using Domain.Models.Entities.MongoDBEntities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class MongoContext : IMongoContext
    {
        public MongoContext(IOptions<MongoDBOptions> options, ILogger<MongoContext> logger)
        {
            Options = options.Value;
            var client = new MongoClient(Options.MongoDefaultConnection);
            if (client == null)
            {
                logger.LogCritical("Mongo Client not found");
                throw new Exception("MongoD Client not found"); 
            }
            logger.LogInformation("Connected Mongo Client on: " + Options.MongoDefaultConnection);
            if (!client.ListDatabaseNames().ToList().Contains(Options.DBName))
            {
                logger.LogCritical("Mongo Database not found");
                throw new Exception("Mongo Database not found");
            }
            Database = client.GetDatabase(Options.DBName);
            logger.LogInformation("Connected Mongo Database: " + Options.DBName);
            Database.CreateCollection(Options.SoldGoodsCollectionName);
            Database.CreateCollection(Options.ReportsCollectionName);
            Database.CreateCollection(Options.OrdersCollectionName);
            Database.CreateCollection(Options.AuditRecordsCollectionName);
            SoldGoods.Indexes.CreateOne(SoldGoodsIndexModel);
        }
        private readonly MongoDBOptions Options;
        private readonly IMongoDatabase Database;
        private readonly CreateIndexModel<SoldProduct> SoldGoodsIndexModel = new CreateIndexModel<SoldProduct>
            (
                keys: Builders<SoldProduct>.IndexKeys.Ascending(p => p.ExpireAt),
                options: new CreateIndexOptions()
                {
                    ExpireAfter = TimeSpan.FromHours(24),
                    Name = "ExpireAtIndex"
                }
            );
        private readonly CreateIndexModel<AuditLogRecord> AuditRecordsIndexModel = new CreateIndexModel<AuditLogRecord>
            (
                keys: Builders<AuditLogRecord>.IndexKeys.Ascending(r => r.ExpireAt),
                options: new CreateIndexOptions()
                {
                    ExpireAfter = TimeSpan.FromDays(7),
                    Name = "ExpireAtIndex"
                }
            );
        public IMongoCollection<SoldProduct> SoldGoods
        {
            get
            {
                return Database.GetCollection<SoldProduct>(Options.SoldGoodsCollectionName);
            }
        }
        public IMongoCollection<Report> Reports
        {
            get
            {
                return Database.GetCollection<Report>(Options.ReportsCollectionName);
            }
        }
        public IMongoCollection<Order> Orders
        {
            get
            {
                return Database.GetCollection<Order>(Options.OrdersCollectionName);
            }
        }
        public IMongoCollection<AuditLogRecord> AuditRecords
        {
            get
            {
                return Database.GetCollection<AuditLogRecord>(Options.AuditRecordsCollectionName);
            }
        }     
    }
}
