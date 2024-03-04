﻿namespace CourseTry1.Dal.Interfaces
{
    public interface IAccountRepository<T>
    {
        Task Add(T user);

        IQueryable<T> GetAll();

        Task<T> GetById(int id);

        Task<T> GetByLogin(string login);
    }
}
