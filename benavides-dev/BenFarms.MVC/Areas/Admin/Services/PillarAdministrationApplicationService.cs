using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Areas.Admin.Models;

namespace BenFarms.MVC.Areas.Admin.Services
{
    public class PillarAdministrationApplicationService
    {
        private readonly MyApplicationDbContext context;
        public PillarAdministrationApplicationService(MyApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<AdministrationServiceResult> create(PillarInputModel inputData)
        {
            
            AdministrationServiceResult result = new AdministrationServiceResult();
                    
                    if(inputData.PillarName.Trim().Length != 0)
                    {
                        if(inputData.PillarDescription.Trim().Length != 0)
                        {
                            Pillar pillar = getTargetPillar(inputData);

                            if (pillar != null)
                            {
                                if (inputData.PillarId == 0)
                                {
                                    context.Pillars.Add(pillar);
                                }
                                else
                                {
                                    context.Entry(pillar).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                            else
                            {
                                result.Errors.Add("Pilar no válido");
                            }
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            result.Errors.Add("Descripción de pilar no válida");
                        }
                    }
                    else
                    {
                        result.Errors.Add("Nombre de pilar no válido");
                    }

            return result;
        }

        private Pillar getTargetPillar(PillarInputModel inputData)
        {
            Pillar pillar;
            if (inputData.PillarId == 0)
            {
                pillar = new Pillar();
            }
            else
            {
                pillar = context.Pillars.Where(b => b.PillarId == inputData.PillarId).FirstOrDefault();
            }

            if(pillar != null)
            {
                var fileService = new FileService();
                var processedFiles = fileService.processFiles(inputData.PillarImage, "~/UploadedFiles");
                pillar.PillarName = inputData.PillarName;
                pillar.PillarDescription = inputData.PillarDescription;
                pillar.PillarLink = inputData.PillarLink;
                pillar.PillarActive = inputData.PillarActive;
                var fileName = inputData.PillarImage[0].FileName;
                pillar.PillarImage = (String.IsNullOrEmpty(fileName) == false &&
                                      processedFiles.ContainsKey(fileName))
                                      ? processedFiles[fileName] : "";
            }
           
            return pillar;
        }
    
    }
}