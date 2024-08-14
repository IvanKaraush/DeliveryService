using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApplicationModels
{
    public class DoesNotExistException : Exception
    {
        public DoesNotExistException(Type entityType)
        {
            Message = entityType.Name + " does not exists";
        }
        public new string Message;
    }
    public class InvalidPageException : Exception
    {
        public string Message = "Page number is invalid or does not exist";
    }
    public class InvalidMarkException : Exception
    {
        public string Message = "Mark can\'t be more than 5 or less than 0";
    }
    public class InvalidFileFormatException : Exception
    {
        public string Message = "Sent file is not a picture";
    }
    public class WasAlreadySetException : Exception
    {
        public WasAlreadySetException(string name) 
        {
            name = name.ToLower();
            name = Char.ToUpper(name[0]) + name.Remove(0, 1);
            Message = name + " was already set";
        }
        public string Message;
    }
    public class IncorrectCardNumberException : Exception
    {
        public string Message = "Card number is incorrect";
    }
}
