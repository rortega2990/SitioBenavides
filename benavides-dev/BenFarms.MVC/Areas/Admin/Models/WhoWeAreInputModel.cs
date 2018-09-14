using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BenavidesFarm.DataModels.Models.Pages;

namespace BenFarms.MVC.Areas.Admin.Models
{
    public class WhoWeAreInputModel
    {
        public WhoWeArePage page { get; set; }
        public HttpFileCollectionBase uploadedFiles { get; set; }
    }
}