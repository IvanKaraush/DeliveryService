using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.VievModels
{
    public class ReportInputModel
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
    }
}
