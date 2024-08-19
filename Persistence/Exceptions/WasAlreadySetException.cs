using Domain.Models.ApplicationModels.Exceptions;
using Microsoft.Build.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Exceptions
{
    public class WasAlreadySetException : ClientException
    {
        public WasAlreadySetException(string name)
        {
            name = name.ToLower();
            Message = $"{Char.ToUpper(name[0])}{name.Remove(0, 1)} was already set";
            Code = HttpStatusCode.Forbidden;
        }
    }
}
