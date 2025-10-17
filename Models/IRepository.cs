using System.Collections.Generic;

namespace UniPlanner.Models
{
    /// <summary>
    /// Generic repository interface for CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        void Add(T item);
        void Update(T item);
        void Delete(int id);
        T GetById(int id);
        IReadOnlyList<T> GetAll();
    }
}

