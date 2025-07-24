using BAL.Specification;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.GenericRepository
{
    public static class SpecificationElevator<T> where T : BaseEntity
    {
        // InputQuery is first part of query => _dbContext.Courses
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecification<T> spec) 
        {
            var Query = InputQuery;
            if (spec.Criteria != null)
            {
                Query = Query.Where(spec.Criteria);
            }
            
            Query = spec.Includes.Aggregate(Query, (currentQuey , InputExpression) => currentQuey.Include(InputExpression));

            return Query;
        }
    }
}
