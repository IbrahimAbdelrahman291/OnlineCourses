using BAL.Interfaces;
using DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.GenericRepository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CoursesDbContext _context;

        public UnitOfWork(CoursesDbContext dbContext)
        {
            _context = dbContext;
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
