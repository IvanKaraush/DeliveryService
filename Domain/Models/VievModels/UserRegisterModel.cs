using Domain.Models.Entities.SQLEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.VievModels
{
    public class UserRegisterModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? TelegramId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public User ToUser()
        {
            return new User { Id =  Id, Name = Name, BirthDate = BirthDate, TelegramId = TelegramId, Login = Login, Password = Password, IsAdmin = false, Bonuses = 0 };
        }
    }
}
