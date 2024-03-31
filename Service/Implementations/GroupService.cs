using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Response;
using CourseTry1.Domain.ViewModels.Shedule;
using CourseTry1.Service.Interfaces;

namespace CourseTry1.Service.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        public async Task<BaseResponse<DaySheduleViewModel>> GetDayShedule(int idShedule, DayOfWeek? dayOfWeek)
        {
            try
            {
                var shedule = await groupRepository.GetDayWeek(idShedule, DayOfWeek.Monday/*dayOfWeek ?? DateTime.Today.DayOfWeek*/);

                if(shedule == null)
                {
                    return new BaseResponse<DaySheduleViewModel>()
                    {
                        Description = "Не найденно распиание",
                        StatusCode = Domain.Enum.StatusCode.DontFindShedule,
                        Data = new DaySheduleViewModel() { }
                    };
                }

                return new BaseResponse<DaySheduleViewModel> 
                {
                    Description = "Успешно получили распиание дня",
                    StatusCode = Domain.Enum.StatusCode.Ok,
                    Data = new DaySheduleViewModel()
                    {
                        Group = shedule.SheduleGroup.NameGroup,
                        Day = dayOfWeek ?? DateTime.Today.DayOfWeek,
                        Subjects = shedule.PairingTime
                    }
                };
            }
            catch
            {
                return new BaseResponse<DaySheduleViewModel>()
                {
                    Data = new DaySheduleViewModel(),
                    Description = "Ошибкав во время получения группы",
                    StatusCode = Domain.Enum.StatusCode.BadRequest
                };
            }
        }
    }
}
