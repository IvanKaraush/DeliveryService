using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApplicationModels
{
    public class HostAuthOptions
    {
        public static string HostAuth = "HostAuth";
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
