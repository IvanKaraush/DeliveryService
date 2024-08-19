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

    public class InvalidFileFormatException : ClientException
    {
        public InvalidFileFormatException()
        {
            Message = "Sent file is not a picture";
            Code = HttpStatusCode.UnsupportedMediaType;
        }
    }
}
