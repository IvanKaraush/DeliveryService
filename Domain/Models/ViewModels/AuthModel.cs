using System.ComponentModel.DataAnnotations;

namespace Domain.Models.VievModels
{
    public class AuthModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
    }
}
