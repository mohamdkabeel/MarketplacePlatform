using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.Specifications
{
    public class BaseSpecification <T>
    {
        public Expression<Func<T,bool>>? Criteria {  get; set; }
        public int Take { get; set; } 
        public int skip { get; set; }

        public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T, bool>>? criteria)
        {
            Criteria = criteria;
        }

    }
}
