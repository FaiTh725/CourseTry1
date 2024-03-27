using CourseTry1.Domain.Enum;

namespace CourseTry1.Domain.ViewModels.User
{
    public class UserViewModel
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public Role Role { get; set; }
    }
}
