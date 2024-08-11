using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApplicationModels
{
    public class MongoDBOptions
    {
        public const string MongoOptions = "MongoOptions";
        public string MongoDefaultConnection {  get; set; }
        public string DBName { get; set; }
        public string SoldGoodsCollectionName { get; set; }
        public string ReportsCollectionName { get; set; }
        public string OrdersCollectionName { get; set; }
        public string AuditRecordsCollectionName { get; set; }
    }
}

