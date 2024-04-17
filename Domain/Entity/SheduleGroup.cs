using CourseTry1.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace CourseTry1.Domain.Entity
{
    public class SheduleGroup
    {
        public long Id { get; set; }

        public string NameGroup { get; set; }

        public Week Week { get; set; }

        public int Cource { get; set; }

        public List<DayWeek> Weeks { get; set; } = new List<DayWeek> ();
    }

    public class DayWeek
    {
        public long Id { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public long SheduleGroupId { get; set; }

        public SheduleGroup SheduleGroup { get; set; }

        public List<Subject> PairingTime { get; set; } = new List<Subject>();
    }

    public class Subject
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Time { get; set; }

        public long DayWeekId {  get; set; }

        public DayWeek DayWeek { get; set; }
    }
}
