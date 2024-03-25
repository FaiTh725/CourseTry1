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
        public DayOfWeek DayOfWeek { get; set; }

        public List<KeyValuePair<string, string>> PairingTime { get; set; }
    }
}
