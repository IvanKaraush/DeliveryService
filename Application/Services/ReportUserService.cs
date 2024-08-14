using Application.Interfaces;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReportUserService : IReportUserService
    {
        public ReportUserService(IReportStore reportStore)
        {
            ReportStore = reportStore;
        }
        private readonly IReportStore ReportStore;
        public async Task AddReport(string message, Guid userId)
        {
            await ReportStore.AddReport(new Report() { Message = message, UserId = userId, Received = DateTime.Now});
        }
    }
}
