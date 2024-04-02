using CourseTry1.Domain.Entity;

namespace CourseTry1.Domain.ViewModels.Shedule
{
    public class DaySheduleViewModel
    {
        public DayOfWeek Day { get; set; }

        public List<SubjectViewModel> Subjects { get; set; }

        public string Group {  get; set; }

        public long Id { get; set; }
    }
}
