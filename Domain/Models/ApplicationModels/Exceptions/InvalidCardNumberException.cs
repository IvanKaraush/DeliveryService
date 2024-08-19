using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApplicationModels.Exceptions
{
    public class InvalidCardNumberException : ClientException
    {
        public InvalidCardNumberException()
        {
            Message = "Card number is incorrect";
            Code = HttpStatusCode.BadRequest;
        }
    }
}
