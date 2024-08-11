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
    /// <summary>
    /// Исключение для некорректного номера страницы
    /// </summary>
    public class InvalidPageException : Exception
    {
        public string Message = "Page number is invalid or does not exist";
    }
    /// <summary>
    /// Исключение для неподходящего формата файла (выбрасывается, если отправлено не фото)
    /// </summary>
    public class InvalidFileFormatException : Exception
    {
        public string Message = "Sent file is not a picture";
    }
    /// <summary>
    /// Исключение для неотправленного файла (шутки ради дал возможность пользователю отправлять пустой файл, чтобы можно было вернуть ошибку 418 - I'm a teapot)
    /// </summary>
    public class EmptyFileException : Exception
    {
        public string Message = "File was not sent";
    }
}
