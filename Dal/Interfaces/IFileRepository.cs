namespace CourseTry1.Dal.Interfaces
{
    public interface IFileRepository
    {
        public Task Add(IFormFile file, IWebHostEnvironment appEnvironment);
    }
}
