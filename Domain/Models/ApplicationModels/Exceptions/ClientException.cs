using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApplicationModels.Exceptions
{    
    /// <summary>
    /// DO NOT THROW this without using initializer for both of fields or you'll earn PROSTATE CANCER
    /// </summary>
    public class ClientException : Exception
    {
        public HttpStatusCode Code;
        public new string? Message;
    }
}
