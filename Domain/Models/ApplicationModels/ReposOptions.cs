using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApplicationModels
{
    public class ReposOptions
    {
        public const string RepositoryOptions = "RepositoryOptions";
        public int AuditExpirationDays { get; set; }
        public int HotGoodsExpirationHours { get; set; }
        public int CacheExpirationMins { get; set; }
    }
}
