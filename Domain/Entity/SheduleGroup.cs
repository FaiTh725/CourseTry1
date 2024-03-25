using System.ComponentModel.DataAnnotations.Schema;

namespace CourseTry1.Domain.Entity
{
    public class SheduleGroup
    {
        public long Id { get; set; }

        public string NameGroup { get; set; }

        public List<DayWeek> Weeks { get; set; }
    }

    public class DayWeek
    {
        public long Id { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public long SheduleGroupId { get; set; }

        [ForeignKey("SheduleGroupId")]
        public SheduleGroup SheduleGroup { get; set; }

        public List<Subject> PairingTime { get; set; }
    }

    public class Subject
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Time { get; set; }

        public long DayWeekId {  get; set; }

        [ForeignKey("DayWeekId")]
        public DayWeek DayWeek { get; set; }
    }
}
