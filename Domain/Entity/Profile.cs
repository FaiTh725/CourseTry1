namespace CourseTry1.Domain.Entity
{
    public class Profile
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }


        public long SheduleGroupId { get; set; }

        public SheduleGroup SheduleGroup { get; set; }
    }
}
