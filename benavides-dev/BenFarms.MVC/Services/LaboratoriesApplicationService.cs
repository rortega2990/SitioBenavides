using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BenavidesFarm.DataModels.Models;
using BenFarms.MVC.Models;

namespace BenFarms.MVC.Services
{
    public class LaboratoriesApplicationService
    {
        private readonly MyApplicationDbContext context;
        public LaboratoriesApplicationService(MyApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<StateViewModel> getStatesWithLaboratories()
        {
            var expression = from laboratory in context.Laboratories
                             where laboratory.Active == true
                             group laboratory by new { Id = laboratory.State.Id, Name = laboratory.State.Name } into state
                             select new StateViewModel()
                             {

                                 Id = state.Key.Id,
                                 Name = state.Key.Name
                             };
            return expression.ToList();
        }

        public IEnumerable<CityViewModel> getCitiesWithLaboratories(int state)
        {
            var expression = from laboratory in context.Laboratories
                             where laboratory.State.Id == state && laboratory.Active == true
                             group laboratory by new { Id = laboratory.City.Id, Name = laboratory.City.Name } into city
                             select new CityViewModel()
                             {

                                 Id = city.Key.Id,
                                 Name = city.Key.Name
                             };

            return expression.ToList();
        }

        public IEnumerable<LaboratoryViewModel> getLaboratoriesForStateAndCity(int state, int city)
        {
            return context.Laboratories.Where(l => l.State.Id == state && l.City.Id == city && l.Active == true).
                   Select(l => new LaboratoryViewModel { Name = l.Name });
        }
    }
}