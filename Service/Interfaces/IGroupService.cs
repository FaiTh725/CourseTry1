using CourseTry1.Domain.Enum;
using CourseTry1.Domain.Response;
using CourseTry1.Domain.ViewModels.Shedule;

namespace CourseTry1.Service.Interfaces
{
    public interface IGroupService
    {
        public Task<BaseResponse<DaySheduleViewModel>> GetDayShedule(int idShedule, DayOfWeek? dayOfWeek, int cource, Week week);

        public Task<BaseResponse<IEnumerable<int>>> GetCources();
    }
}
