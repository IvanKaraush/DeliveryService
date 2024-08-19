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
using Persistence.Exceptions;
using Infrastructure.Interfaces;

namespace Persistence.Repository
{
    public class ReportRepository : IReportStore
    {
        public ReportRepository(IMongoContext context) 
        {
            _context = context;
        }
        private readonly IMongoContext _context;

        public async Task<Report> GetReportById(DateTime id)
        {
            Report? report = await _context.Reports.Find(new BsonDocument("Received", id)).FirstOrDefaultAsync();
            if (report == null)
                throw new DoesNotExistException(typeof(Report));
            return report;
        }

        public async Task<List<DateTime>> GetReportsIds()
        {
            return await _context.Reports.AsQueryable().Select(r => r.Received).ToListAsync();
        }

        public async Task AddReport(Report report)
        {
            report.Received = DateTime.Now;
            await _context.Reports.InsertOneAsync(report);
        }

        public async Task RemoveReport(DateTime id)
        {
            await _context.Reports.DeleteOneAsync(new BsonDocument("Received", id));
        }
    }
}
