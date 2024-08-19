using Domain.Models.ApplicationModels.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Application.Exceptions
{
    public class InvalidMarkException : ClientException
    {
        public InvalidMarkException()
        {
            Message = "Mark can\'t be more than 5 or less than 0";
            Code = HttpStatusCode.BadRequest;
        }
    }
}
