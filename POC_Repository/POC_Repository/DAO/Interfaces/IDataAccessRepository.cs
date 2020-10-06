using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Interfaces
{
    public interface IDataAccessRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int Id);
        Task<bool> CreateEntity(T Entity);
        Task<bool> UpdateEntity(T Entity);
        Task<bool> DeleteEntity(T Entity);
        Task<bool> ExistEntity(int Id);

    }
}
