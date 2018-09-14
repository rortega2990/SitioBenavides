using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Services
{
    public class AdministrationServiceResult
    {
        public List<String> Errors { get; set; } = new List<string>();
        public bool IsValid { get { return this.Errors.Count == 0; } }
        public object ResultObject { get; set; }
    }
}