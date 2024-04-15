using CourseTry1.Domain.Entity;
using CourseTry1.Domain.Enum;

namespace CourseTry1.Domain.ViewModels.Shedule
{
    public class DaySheduleViewModel
    {
        public DayOfWeek Day { get; set; }

        public List<SubjectViewModel> Subjects { get; set; }

        public string Group {  get; set; }

        public long Id { get; set; }

        public IEnumerable<int> Cources { get; set; } = new List<int>();

        public Week CurrentWeek { get; set; } = Week.first;

        public int SelectedCource { get; set; } = 1;
    }
}
