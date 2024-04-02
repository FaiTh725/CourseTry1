using CourseTry1.Domain.Entity;

namespace CourseTry1.Dal.Interfaces
{
    public interface IExcelFileRepository
    {
        IQueryable<SheduleGroup> GetAllGroups();

        Task Save(List<SheduleGroup> sheduleGroups);

        Task Clear();
    }
}
