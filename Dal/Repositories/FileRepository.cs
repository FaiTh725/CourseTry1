using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;

namespace CourseTry1.Dal.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext context;

        public FileRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add(IFormFile file, IWebHostEnvironment appEnvironment)
        {
            string path = "/files/" + file.FileName;

            using (var fileStream = new FileStream(appEnvironment.WebRootPath+path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            context.Files.Add(new ExcelFile() {
                Name = file.FileName,
                Path = path
            });

            await context.SaveChangesAsync();
        }
    }
}
