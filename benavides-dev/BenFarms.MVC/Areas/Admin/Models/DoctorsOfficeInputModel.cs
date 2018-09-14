using BenavidesFarm.DataModels.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Models
{
    public class DoctorsOfficeInputModel
    {
        public DoctorsOfficePage page { get; set; }

        public HttpFileCollectionBase uploadedFiles { get; set; }

    }
}