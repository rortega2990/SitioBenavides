using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenFarms.MVC.Areas.Admin.Validation
{
    public abstract class Specification<T> where T:class
    {
        public virtual bool isSatisfiedBy(T entity)
        {
            return this.brokenRules(entity).Count == 0;
        }
        public abstract List<string> brokenRules(T entity);
    }
}
