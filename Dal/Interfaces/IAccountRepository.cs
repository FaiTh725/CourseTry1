namespace CourseTry1.Dal.Interfaces
{
    public interface IAccountRepository<T>
    {
        Task Add(T user);

        IQueryable<T> GetAll();
    }
}
