using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BenFarms.MVC.Models;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using LinqKit;

namespace BenFarms.MVC.Services
{
    public class BranchesApplicationService
    {
        private readonly MyApplicationDbContext context;
        public BranchesApplicationService(MyApplicationDbContext context)
        {
            this.context = context;
        }
        public BranchSearchResultViewModel getBranchesByLocaltionriteria(BranchSearchInputModel searchCriteria)
        {
            BranchSearchResultViewModel result = new BranchSearchResultViewModel();

            var baseQueryable = context.Branchs.AsQueryable().Where(b => b.BranchActive == true &&
                                               b.State.Id == searchCriteria.State &&
                                               b.City.Id == searchCriteria.City);

            var builder = PredicateBuilder.New<Branch>();            


            //var queryableBase = context.Branchs.AsQueryable();


            /*if ((searchCriteria.Branch24Hours == 1 && searchCriteria.BranchConsult == 1) ||
                (searchCriteria.Branch24Hours == 0 && searchCriteria.BranchConsult == 0))
            {
                result.DoctorOfficeCriteriaEnabled = true;
                result.TwentyFourHoursCriteriaEnabled = true;
            }
            else
            {
                if (searchCriteria.Branch24Hours == 1)
                {
                    //result.TwentyFourHoursBranches = this.getBranchesByConsultType(baseQueryable, false).ToList();
                    baseQueryable = baseQueryable.Where(b => b.BranchConsult == false);
                    result.TwentyFourHoursCriteriaEnabled = true;
                    result.DoctorOfficeCriteriaEnabled = false;
                }
                if (searchCriteria.BranchConsult == 1)
                {
                    baseQueryable = baseQueryable.Where(b => b.BranchConsult == true);
                    result.DoctorOfficeCriteriaEnabled = true;
                    result.TwentyFourHoursCriteriaEnabled = false;
                }
            }    */

            
            if(searchCriteria.Branch24Hours == "on")
            {
                //baseQueryable = baseQueryable.Where(b => b.BranchTwentyFourHours == true);
                builder.Or(b => b.BranchTwentyFourHours == true);
                result.TwentyFourHoursCriteriaEnabled = true;
            }
            if (searchCriteria.BranchConsult == "on")
            {
                //baseQueryable = baseQueryable.Where(b => b.BranchConsult == true);
                builder.Or(b => b.BranchConsult == true);
                result.DoctorOfficeCriteriaEnabled = true;
            }
            if (searchCriteria.BranchFose == "on")
            {
                //baseQueryable = baseQueryable.Where(b => b.BranchFose == true);
                builder.Or(b => b.BranchFose == true);
                result.BranchFoseCriteriaEnabled = true;
            }

            result.Branches = baseQueryable.Where(builder).Select(b => new BranchViewModel
                                                            {
                                                                Id = b.BranchId,
                                                                Name = b.BranchName,
                                                                Address = b.BranchAddress,
                                                                Latitude = b.BranchLatitude,
                                                                Longitude = b.BranchLongitude,
                                                                City = b.City.Name,
                                                                State = b.State.Name,
                                                                Hour = (b.BranchHour1!=null && b.BranchHour2!=null)? b.BranchHour1 + " a " + b.BranchHour2:""
                                                             }).ToList();

            return result;
        }



        public BranchSearchResultViewModel getBranchesByLocaltionriteria2(BranchSearchInputModel searchCriteria)
        {
            BranchSearchResultViewModel result = new BranchSearchResultViewModel();

            //string[] BrancCecoList = new string[] {"2L0131","2L0132","2L0139","2L0141","2L0143","2L0152","2L0155","2L0162","2L0195","2L0197","2L0207","2L0219","2L0227","2L0236","2L0251","2L0253","2L0262",
            //"2L0265","2L0315","2L0316","2L0317","2L0326","2L0614","2L0634","2L0654","2L0655","2L0698","2L0770","2L0778","2L0809","2L0895","2L1056","2L1058","2L1087","2L1127",
            //"2L1137","2L1146","2L1157","2L1199","2L1204","2L1228","2L1229","2L1260","2L1279","2L1301","2L1325" };

            string[] BrancCecoList = new string[] { "2L0131", "2L0132", "2L0139", "2L0141", "2L0143", "2L0152", "2L0195", "2L0227", "2L0236", "2L0262", "2L0265", "2L0614", "2L1058", "2L1127" };

            var baseQueryable = context.Branchs.AsQueryable().Where(b => b.BranchActive == true &&
                                               b.City.Id == searchCriteria.City
                                               && BrancCecoList.Contains(b.BranchCeco)
                                               );

            var builder = PredicateBuilder.New<Branch>();


            //var queryableBase = context.Branchs.AsQueryable();


            /*if ((searchCriteria.Branch24Hours == 1 && searchCriteria.BranchConsult == 1) ||
                (searchCriteria.Branch24Hours == 0 && searchCriteria.BranchConsult == 0))
            {
                result.DoctorOfficeCriteriaEnabled = true;
                result.TwentyFourHoursCriteriaEnabled = true;
            }
            else
            {
                if (searchCriteria.Branch24Hours == 1)
                {
                    //result.TwentyFourHoursBranches = this.getBranchesByConsultType(baseQueryable, false).ToList();
                    baseQueryable = baseQueryable.Where(b => b.BranchConsult == false);
                    result.TwentyFourHoursCriteriaEnabled = true;
                    result.DoctorOfficeCriteriaEnabled = false;
                }
                if (searchCriteria.BranchConsult == 1)
                {
                    baseQueryable = baseQueryable.Where(b => b.BranchConsult == true);
                    result.DoctorOfficeCriteriaEnabled = true;
                    result.TwentyFourHoursCriteriaEnabled = false;
                }
            }    */


 

            result.Branches = baseQueryable.Select(b => new BranchViewModel
            {
                Id = b.BranchId,
                Name = b.BranchName,
                Address = b.BranchAddress,
                Latitude = b.BranchLatitude,
                Longitude = b.BranchLongitude,
                City = b.City.Name,
                State = b.State.Name,
                Hour = (b.BranchHour1 != null && b.BranchHour2 != null) ? b.BranchHour1 + " a " + b.BranchHour2 : ""
            }).ToList();

            return result;
        }


    }
}