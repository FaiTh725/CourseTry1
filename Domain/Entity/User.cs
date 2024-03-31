using CourseTry1.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseTry1.Domain.Entity
{
    public class User
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; } = Role.User;
/*
        public long? ProfileId {  get; set; }

        public Profile? Profile { get; set; }*/
    }
}
