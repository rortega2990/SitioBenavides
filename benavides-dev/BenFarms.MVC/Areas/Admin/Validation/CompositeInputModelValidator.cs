using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Validation
{
    /*public class CompositeInputModelValidator<I, P> : InputModelValidator<I, P> where I:class where P:class
    {
        private List<InputModelValidator<I, P>> validators;
        public CompositeInputModelValidator(List<InputModelValidator<I,P>> validators)
        {
            this.validators = validators;
        }
        public override List<string> brokenRules(I inputModel, P referencePage, IEnumerable<string> uploadedFiles)
        {
            List<string> result = new List<string>();

            validators.ForEach(v => result.AddRange(v.brokenRules(inputModel, referencePage,uploadedFiles)));

            return result;
        }
    }*/
}