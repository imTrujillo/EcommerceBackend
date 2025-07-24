using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Domain.Entities;

namespace Tienda.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        Task<T> AddAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<bool> GetAnyAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<T> UpdateAsync(T entity, int id);
    }
}
