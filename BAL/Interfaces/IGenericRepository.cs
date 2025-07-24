using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<T> GetByIdAsync(int id);
        public Task<ICollection<T>> GetAllAsync();
        public Task AddAsync(T item);
        public void Update(T item);
        public void Delete(int id);
        public Task<int> CompleteAsync();

    }
}
