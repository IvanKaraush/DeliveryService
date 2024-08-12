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
        public MongoContext(IOptions<MongoDBOptions> mongoOptions, IOptions<ReposOptions> repositoryOptions, ILogger<MongoContext> logger)
        {
            MongoOptions = mongoOptions.Value;
            var client = new MongoClient(MongoOptions.MongoDefaultConnection);
            if (client == null)
            {
                logger.LogCritical("Mongo Client not found");
                throw new Exception("Mongo Client not found"); 
            }
            logger.LogInformation("Connected Mongo Client on: " + MongoOptions.MongoDefaultConnection);
            if (!client.ListDatabaseNames().ToList().Contains(MongoOptions.DBName))
            {
                logger.LogCritical("Mongo Database not found");
                throw new Exception("Mongo Database not found");
            }
            Database = client.GetDatabase(MongoOptions.DBName);
            logger.LogInformation("Connected Mongo Database: " + MongoOptions.DBName);
            Database.CreateCollection(MongoOptions.SoldGoodsCollectionName);
            Database.CreateCollection(MongoOptions.ReportsCollectionName);
            Database.CreateCollection(MongoOptions.OrdersCollectionName);
            Database.CreateCollection(MongoOptions.AuditRecordsCollectionName);
            SoldGoods.Indexes.CreateOne(new CreateIndexModel<SoldProduct>
            (
                keys: Builders<SoldProduct>.IndexKeys.Ascending(p => p.ExpireAt),
                options: new CreateIndexOptions()
                {
                    ExpireAfter = TimeSpan.FromHours(repositoryOptions.Value.HotGoodsExpirationHours),
                    Name = "ExpireAtIndex"
                }
            ));
            AuditRecords.Indexes.CreateOne(new CreateIndexModel<AuditLogRecord>
            (
                keys: Builders<AuditLogRecord>.IndexKeys.Ascending(r => r.ExpireAt),
                options: new CreateIndexOptions()
                {
                    ExpireAfter = TimeSpan.FromDays(repositoryOptions.Value.AuditExpirationDays),
                    Name = "ExpireAtIndex"
                }
            ));
        }
        private readonly MongoDBOptions MongoOptions;
        private readonly IMongoDatabase Database;
        public IMongoCollection<SoldProduct> SoldGoods
        {
            get
            {
                return Database.GetCollection<SoldProduct>(MongoOptions.SoldGoodsCollectionName);
            }
        }
        public IMongoCollection<Report> Reports
        {
            get
            {
                return Database.GetCollection<Report>(MongoOptions.ReportsCollectionName);
            }
        }
        public IMongoCollection<Order> Orders
        {
            get
            {
                return Database.GetCollection<Order>(MongoOptions.OrdersCollectionName);
            }
        }
        public IMongoCollection<AuditLogRecord> AuditRecords
        {
            get
            {
                return Database.GetCollection<AuditLogRecord>(MongoOptions.AuditRecordsCollectionName);
            }
        }     
    }
}
