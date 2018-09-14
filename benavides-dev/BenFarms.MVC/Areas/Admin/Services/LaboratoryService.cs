using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BenavidesFarm.DataModels.Models;
using BenFarms.MVC.Areas.Admin.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BenFarms.MVC.Areas.Admin.Services
{
    public class LaboratoryService
    {
        private MyApplicationDbContext context;

        public LaboratoryService(MyApplicationDbContext context)
        {
            this.context = context;
        }

        public AdministrationServiceResult addOrUpdateLaboratory(LaboratoryInputModel inputData)
        {
            AdministrationServiceResult result = new AdministrationServiceResult();

            if(inputData != null)
            {
                var city = context.Municipios.Where(c => c.Id == inputData.CityId).FirstOrDefault();
                if (city == null)
                {
                    result.Errors.Add("Municipio no válido");
                }

                var state = context.Estados.Where(st => st.Id == inputData.StateId).FirstOrDefault();
                if (state == null)
                {
                    result.Errors.Add("Estado no válido");
                }

                if (result.IsValid)
                {
                    AdministrationServiceResult operationResult = null;
                    if (inputData.Id > 0)
                    {
                        operationResult = this.updateLaboratory(inputData, city, state);
                    }
                    else
                    {
                        operationResult = this.createLaboratory(inputData, city, state);
                    }
                    context.SaveChanges();
                    result.Errors.AddRange(operationResult.Errors);
                }
            }
            else
            {
                result.Errors.Add("Datos de entrada no válidos");
            }
            
            return result;
        }

        public AdministrationServiceResult remove(int? id)
        {
            AdministrationServiceResult result = new AdministrationServiceResult();

            if(id != null)
            {
                var laboraotry = context.Laboratories.Where(l => l.ID == id.Value).FirstOrDefault();
                if(laboraotry != null)
                {
                    context.Laboratories.Remove(laboraotry);
                    context.SaveChanges();
                }
                else
                {
                    result.Errors.Add("Laboratorio no válido");
                }
            }
            else
            {
                result.Errors.Add("Datos de entrada no válidos");
            }

            return result;
        }

        public async Task<IEnumerable<LaboratoryViewModel> > getLaboratories()
        {
            var laboratories =  await context.Laboratories.Select(l => new LaboratoryViewModel()
                                                    {
                                                        Id = l.ID,
                                                        Name = l.Name,
                                                        Active = l.Active,
                                                        City = l.City.Name,
                                                        CityId = l.City.Id,
                                                        State = l.State.Name,
                                                        StateId = l.State.Id
                                                    }).ToListAsync();
            return laboratories;
        }

        public LaboratoryInputModel getInputModelForId(int? id)
        {
            if(id != null)
            {
                var laboratory = context.Laboratories.Where(l => l.ID == id.Value).Select(l => new LaboratoryInputModel() {
                                                            Id = l.ID,
                                                            Name = l.Name,
                                                            CityId = l.City.Id,
                                                            StateId = l.State.Id
                                                        }).FirstOrDefault();
                if(laboratory != null)
                {
                    return laboratory;
                }

                return new LaboratoryInputModel();
            }
            return new LaboratoryInputModel();
        }
                
        private AdministrationServiceResult createLaboratory(LaboratoryInputModel inputData, Municipios city, Estados state)
        {
            AdministrationServiceResult result = new AdministrationServiceResult();
            Laboratory laboratory = new Laboratory
            {
                Name = inputData.Name,
                Active = inputData.Active,
                City = city,
                State = state
            };
            context.Laboratories.Add(laboratory);
            return result;
        }

        private AdministrationServiceResult updateLaboratory(LaboratoryInputModel inputData, Municipios city, Estados state)
        {
            AdministrationServiceResult result = new AdministrationServiceResult();
            var laboratory = context.Laboratories.Where(l => l.ID == inputData.Id).FirstOrDefault();
            if (laboratory != null)
            {
                laboratory.Name = inputData.Name;
                laboratory.Active = inputData.Active;
                laboratory.City = city;
                laboratory.State = state;
                context.Entry(laboratory).State = EntityState.Modified;
            }
            else
            {
                result.Errors.Add("Laboratorio no válido");
            }
            return result;
        }

    }
}