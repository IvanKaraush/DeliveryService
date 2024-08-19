using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApplicationModels
{
    public class HostAuthOptions
    {
        public const string OptionsName = "HostAuth";
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}
