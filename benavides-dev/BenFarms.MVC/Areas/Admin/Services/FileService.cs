using BenavidesFarm.DataModels.Models.Pages.Sections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Services
{
    public class FileService
    {
        public Dictionary<string, string> processFiles(HttpFileCollectionBase files, string physicalDirectory)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            //for(int i = 0; i < files.Count; ++i)
            foreach(string key in files.AllKeys)
            {
                var postedFile = files.Get(key);
                var fileName = $"{Path.GetFileNameWithoutExtension(postedFile.FileName)}_{DateTime.Now.Ticks}{postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'))}";
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(physicalDirectory), fileName);

                if (!postedFile.FileName.StartsWith("/"))
                {
                    postedFile.SaveAs(fileSavePath);
                    
                    if(physicalDirectory.EndsWith("/") == false && physicalDirectory.EndsWith("\\") == false)
                    {
                        physicalDirectory += "/";
                    }

                    result[key] = physicalDirectory + fileName;
                }
            }
            return result;
        }

        public void mapFilesToSectionForCreateOrUpdate(List<DoctorsOfficePageSection> targetSection, List<DoctorsOfficePageSection> referenceSection, Dictionary<string, string> processedFiles)
        {
            targetSection.ForEach(ts => ts.ImageFileName = (processedFiles.ContainsKey(ts.ImageFileName) ?
                                                            processedFiles[ts.ImageFileName] :
                                                            referenceSection.Where(rs => rs.Id.ToString() == ts.ImageFileName).First().ImageFileName));
        }

        public void mapFilesToSectionForCreateOrUpdate(List<WhoWeAreTitledSection> targetSection, List<WhoWeAreTitledSection> referenceSection, Dictionary<string, string> processedFiles)
        {
            targetSection.ForEach(ts => ts.ImageFileName = (processedFiles.ContainsKey(ts.ImageFileName) ?
                                                            processedFiles[ts.ImageFileName] :
                                                            referenceSection.Where(rs => rs.Id.ToString() == ts.ImageFileName).First().ImageFileName));
        }

        public void mapFilesToSectionForCreateOrUpdate(List<WhoWeAreSimpleRowItemWithImage> targetSection, List<WhoWeAreSimpleRowItemWithImage> referenceSection, Dictionary<string, string> processedFiles)
        {
            targetSection.ForEach(ts => ts.ImageFileName = (processedFiles.ContainsKey(ts.ImageFileName) ?
                                                            processedFiles[ts.ImageFileName] :
                                                            referenceSection.Where(rs => rs.Id.ToString() == ts.ImageFileName).First().ImageFileName));
        }

        public void mapFilesToSectionForPreview(List<ISectionWithImage> targetSection, List<ISectionWithImage> referenceSection, IEnumerable<String> inputFiles)
        {
            targetSection.ForEach(ts => ts.ImageFileName = (inputFiles.Contains(ts.ImageFileName) ?
                                                            ts.ImageFileName :
                                                            referenceSection.Where(rs => rs.Id.ToString() == ts.ImageFileName).First().ImageFileName));
        }
    }
}