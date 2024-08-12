using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.VievModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReportUserService
    {
        Task AddReport(ReportInputModel report);
    }
}
