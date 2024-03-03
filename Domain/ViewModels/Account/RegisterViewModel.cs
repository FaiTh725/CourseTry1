using System.ComponentModel.DataAnnotations;

namespace CourseTry1.Domain.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Обязательно укажите логин")]
        [MaxLength(10, ErrorMessage = "Максимальная длинна 10 символов")]
        [MinLength(4, ErrorMessage = "Минимальная длинна 4 символа")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Укажите пароль")]
        [MaxLength(10, ErrorMessage = "Максимальная длинна 10 миволов")]
        [MinLength(4, ErrorMessage = "Минимальная длинна 4 символа")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Повтрорите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
