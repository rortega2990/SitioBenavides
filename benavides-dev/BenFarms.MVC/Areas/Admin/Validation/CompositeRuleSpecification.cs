using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Validation
{
    public class CompositeRuleSpecification<T> : Specification<T> where T : class
    {
        private List<Specification<T>> specifications;
        public CompositeRuleSpecification(List<Specification<T>> specifications)
        {
            this.specifications = specifications;
        }
        public override List<string> brokenRules(T entity)
        {
            List<string> result = new List<string>();

            specifications.ForEach(s => result.AddRange(s.brokenRules(entity)));

            return result;
        }
    }
}