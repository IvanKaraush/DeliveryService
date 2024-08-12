using Domain.Models.Entities.SQLEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.VievModels
{
    public class UserOutputModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? TelegramId { get; set; }
        public decimal Bonuses { get; set; }
        public bool IsAdmin { get; set; }
    }
}
