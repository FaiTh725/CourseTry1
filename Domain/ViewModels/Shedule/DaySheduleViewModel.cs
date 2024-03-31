using CourseTry1.Domain.Entity;

namespace CourseTry1.Domain.ViewModels.Shedule
{
    public class DaySheduleViewModel
    {
        public DayOfWeek Day { get; set; }

        public List<Subject> Subjects { get; set; }

        public string Group {  get; set; }
    }
}
