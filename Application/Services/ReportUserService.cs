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
            _reportStore = reportStore;
        }
        private readonly IReportStore _reportStore;
        public async Task AddReport(string message, Guid userId)
        {
            await _reportStore.AddReport(new Report() { Message = message, UserId = userId, Received = DateTime.Now});
        }
    }
}
