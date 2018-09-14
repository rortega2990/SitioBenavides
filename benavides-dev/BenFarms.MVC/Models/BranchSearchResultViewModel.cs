using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Models
{
    public class BranchSearchResultViewModel
    {
       public BranchSearchResultViewModel()
        {
            Branches = new List<BranchViewModel>();
        }
        public List<BranchViewModel> Branches { get; set; }
        public bool DoctorOfficeCriteriaEnabled { get; set; }
        public bool TwentyFourHoursCriteriaEnabled { get; set; }
        public bool BranchFoseCriteriaEnabled { get; set; }
    }
}