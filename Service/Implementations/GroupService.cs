using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Enum;
using CourseTry1.Domain.Response;
using CourseTry1.Domain.ViewModels.Shedule;
using CourseTry1.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseTry1.Service.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        
        public async Task<BaseResponse<DaySheduleViewModel>> GetDayShedule(int idShedule, DayOfWeek? dayOfWeek, int cource, Week week)
        {
            try
            {
                //var shedule = await groupRepository.GetDayWeek(idShedule, dayOfWeek ?? (DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? DayOfWeek.Monday : DateTime.Today.DayOfWeek));
                var getShedule = await groupRepository.GetGroupById(idShedule);
                var shedule = await groupRepository.GetDayByPram(getShedule.NameGroup, dayOfWeek ?? (DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? DayOfWeek.Monday : DateTime.Today.DayOfWeek), cource, week);

                if(shedule == null)
                {
                    return new BaseResponse<DaySheduleViewModel>()
                    {
                        Description = "Не найденно распиание",
                        StatusCode = Domain.Enum.StatusCode.DontFindShedule,
                        Data = new DaySheduleViewModel() { }
                    };
                }
                var cources = await GetCources();

                return new BaseResponse<DaySheduleViewModel>
                {
                    Description = "Успешно получили распиание дня",
                    StatusCode = Domain.Enum.StatusCode.Ok,
                    Data = new DaySheduleViewModel()
                    {
                        Group = shedule.SheduleGroup.NameGroup,
                        Day = dayOfWeek ?? (DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? DayOfWeek.Monday : DateTime.Today.DayOfWeek),
                        Subjects = shedule.PairingTime.Select(c => new SubjectViewModel(c)).ToList(),
                        Id = idShedule,
                        SelectedCource = cource,
                        CurrentWeek = week,
                        Cources = cources.Data
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

        public async Task<BaseResponse<IEnumerable<int>>> GetCources()
        {
            try
            {
                return new BaseResponse<IEnumerable<int>>
                {
                    Data = await groupRepository.GetCources(),
                    Description = "Успешно получили список курсов",
                    StatusCode = StatusCode.Ok,
                };
            }
            catch
            {
                return new BaseResponse<IEnumerable<int>>()
                {

                    Description = "Ошибка во время выполнения",
                    StatusCode = StatusCode.BadRequest,
                    Data = new List<int>()
                };
            }
        }
    }
}
