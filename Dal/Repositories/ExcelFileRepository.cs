using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.ConditionalFormatting.Contracts;

namespace CourseTry1.Dal.Repositories
{
    public class ExcelFileRepository : IExcelFileRepository
    {
        private readonly AppDbContext context;

        public ExcelFileRepository(AppDbContext context)
        {
            this.context = context;
        }


        public IQueryable<SheduleGroup> GetAllGroups()
        {
            return context.SheduleGroups;
        }

        public async Task Save(List<SheduleGroup> sheduleGroups)
        {
            foreach (var group in sheduleGroups)
            {
                context.SheduleGroups.Add(group);
            }

            await context.SaveChangesAsync();
        }

        public async Task Clear()
        {
            context.SheduleGroups.RemoveRange(GetAllGroups());

            await context.SaveChangesAsync();
        }
    }
}
