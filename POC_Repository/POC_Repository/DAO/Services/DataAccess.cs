using DAO.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Services
{
    public class DataAccess<T> : IDataAccessRepository<T> where T : class, IEntity
    {
        BD_Context BD = null;

        public DataAccess()
        {
            BD = new BD_Context();
        }

        public async Task<T> GetById(int Id)
        {
            try
            {
                T Result = await BD.Set<T>().FindAsync(Id);
                return Result;
            }
            catch (Exception Error)
            {
                return null;
            }
        }

        public async Task<List<T>> GetAll()
        {
            try
            {
                List<T> Lista = null;
                Lista = await BD.Set<T>().AsNoTracking().ToListAsync();
                return (Lista);
            }
            catch (Exception Error)
            {
                return null;
            }
        }

        public async Task<bool> CreateEntity(T Entity)
        {
            try
            {
                BD.Set<T>().Add(Entity);
                int Result = await BD.SaveChangesAsync();
                if (Result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Error)
            {
                return false;
            }
        }

        public async Task<bool> UpdateEntity(T Entity)
        {
            try
            {
                BD.Set<T>().Update(Entity);
                int Result = await BD.SaveChangesAsync();
                if (Result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Error)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEntity(T Entity)
        {
            try
            {
                BD.Set<T>().Remove(Entity);
                int Result = await BD.SaveChangesAsync();

                if (Result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Error)
            {
                return false;
            }
        }

        public async Task<bool> ExistEntity(int Id)
        {
            try
            {
                return await BD.Set<T>().AnyAsync(x => x.Id == Id);
            }
            catch (Exception Error)
            {
                return false;
            }
        }

    }
}
