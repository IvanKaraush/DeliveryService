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
    public class DoesNotExistException : ClientException
    {
        public DoesNotExistException(Type entityType)
        {
            Message = $"{entityType.Name} does not exists";
            Code = HttpStatusCode.NotFound;
        }
    }
}
