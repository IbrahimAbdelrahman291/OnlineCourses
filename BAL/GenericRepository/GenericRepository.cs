using BAL.Interfaces;
using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CoursesDbContext _dbContext;

        public GenericRepository(CoursesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T item)
        {
            await _dbContext.AddAsync(item);
        }

        public Task<int> CompleteAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var item = _dbContext.Set<T>().Find(id);
            _dbContext.Set<T>().Remove(item!);
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        {
            _dbContext.Update(item);
        }
    }
}
