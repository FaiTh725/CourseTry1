using System.ComponentModel.DataAnnotations.Schema;

namespace CourseTry1.Domain.Entity
{
    public class Profile
    {
        public long Id { get; set; }

        public List<int> Cources { get; set; } = new List<int>();
        
        public long? UserId { get; set; }

        public User? User { get; set; }

        public List<SheduleGroup> Groups { get; set; } = new List<SheduleGroup>();
    }
}
