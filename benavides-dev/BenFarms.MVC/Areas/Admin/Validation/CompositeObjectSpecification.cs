using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Validation
{
    public class CompositeObjectSpecification<T> where T: class
    {
        private List<T> objects;
        public CompositeObjectSpecification(List<T> objects)
        {
            this.objects = objects;
        }

        public List<string> brokenRules(Specification<T> specification)
        {
            List<string> result = new List<string>();

            objects.ForEach(o => result.AddRange(specification.brokenRules(o)));

            return result;
        }
    }
}