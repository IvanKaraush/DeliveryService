using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApplicationModels
{
    public class AuthOptions
    {
        public const string Issuer = "DeliveryServiceIssuer";
        public const string Client = "DeliveryServiceClient"; 
        public const int Lifetime = 10;
        const string Key = "Pj6pstoijkGh45pdTmpoiJkp964oMmhkt";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
