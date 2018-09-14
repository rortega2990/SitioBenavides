using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BenFarms.MVC.Models;
using BenFarms.MVC.Services;
using BenavidesFarm.DataModels.Models;

namespace BenFarms.MVC.Controllers
{
    [RoutePrefix("v1/stateslaboratories")]
    public class StatesLaboratoriesController : ApiController
    {
        private readonly MyApplicationDbContext context;
        LaboratoriesApplicationService service;

        public StatesLaboratoriesController()
        {
            this.context = new MyApplicationDbContext();
            this.service = new LaboratoriesApplicationService(this.context);
        }
        [HttpGet]
        public IEnumerable<StateViewModel> GetStatesWithLaboratories()
        {
            return service.getStatesWithLaboratories();
        }

        [HttpGet]
        public IEnumerable<CityViewModel> GetCitiesWithLaboratories(int state)
        {
            return service.getCitiesWithLaboratories(state);
        }

        [HttpGet]
        public IEnumerable<LaboratoryViewModel> getLaboratoriesForStateAndCity(int state, int city)
        {
            return service.getLaboratoriesForStateAndCity(state, city);
        }
    }
}