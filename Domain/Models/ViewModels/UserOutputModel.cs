﻿using Domain.Models.Entities.SQLEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.VievModels
{
    public class UserOutputModel
    {
        public UserOutputModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
            BirthDate = user.BirthDate;
            TelegramId = user.TelegramId;
            Bonuses = user.Bonuses;
            IsAdmin = user.IsAdmin;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? TelegramId { get; set; }
        public decimal Bonuses { get; set; }
        public bool IsAdmin { get; set; }
    }
}
