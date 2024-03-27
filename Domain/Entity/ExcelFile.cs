namespace CourseTry1.Domain.Entity
{
    public class ExcelFile
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
