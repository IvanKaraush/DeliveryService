using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApplicationModels
{
    public class MongoDBOptions
    {
        public const string OptionsName = "MongoOptions";
        public required string MongoDefaultConnection {  get; set; }
        public required string DBName { get; set; }
        public required string SoldGoodsCollectionName { get; set; }
        public required string ReportsCollectionName { get; set; }
        public required string OrdersCollectionName { get; set; }
        public required string AuditRecordsCollectionName { get; set; }
    }
}

