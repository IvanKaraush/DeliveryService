using Domain.Models.ApplicationModels;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class ReportRepository : IReportStore
    {
        public ReportRepository(IMongoContext context) 
        {
            Context = context;
        }
        private readonly IMongoContext Context;
        public async Task AddReport(Report report)
        {
            report.Received = DateTime.Now;
            await Context.Reports.InsertOneAsync(report);
        }

        public async Task<Report> GetReportById(DateTime id)
        {
            Report? report = await Context.Reports.Find(new BsonDocument("Received", id)).FirstOrDefaultAsync();
            if (report == null)
                throw new DoesNotExistException(typeof(Report));
            return report;
        }

        public async Task<List<DateTime>> GetReportsIds()
        {
            return await Context.Reports.AsQueryable().Select(r => r.Received).ToListAsync();
        }

        public async Task RemoveReport(DateTime id)
        {
            await Context.Reports.DeleteOneAsync(new BsonDocument("Received", id));
        }
    }
}
