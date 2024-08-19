﻿using Domain.Models.ApplicationModels.Exceptions;
using Microsoft.Build.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Exceptions
{
    public class InvalidPageException : ClientException
    {
        public InvalidPageException()
        {
            Message = "Page number is invalid or does not exist";
            Code = HttpStatusCode.BadRequest;
        }
    }
}
