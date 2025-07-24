using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Specification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T , bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
    }
}
