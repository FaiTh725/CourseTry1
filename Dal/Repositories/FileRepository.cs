using Azure.Core;
using CourseTry1.Dal.Interfaces;
using CourseTry1.Domain.Entity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

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


        public IQueryable<ExcelFile> GetAll()
        {
            return context.Files;
        }

        public async Task<ExcelFile> GetByName(string name)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task DeleteFile(ExcelFile excelFile)
        {
            context.Files.Remove(excelFile);
            await context.SaveChangesAsync();
        }

        public async Task<ExcelFile> UpdateFile(ExcelFile excelFile)
        {
            context.Files.Update(excelFile);
            await context.SaveChangesAsync();

            return excelFile;
        }

        public async Task<ExcelFile> GetById(int id)
        {
            return await context.Files.FirstOrDefaultAsync(x => x.Id == id);   
        }
    }
}
