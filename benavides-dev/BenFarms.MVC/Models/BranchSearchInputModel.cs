using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Models
{
    public class BranchSearchInputModel
    {
        public BranchSearchInputModel()
        {

        }
        public int City { get; set; }
        public int State { get; set; }
        public String BranchConsult { get; set; }
        public String Branch24Hours { get; set; }
        public String BranchFose { get; set; }
    }
}