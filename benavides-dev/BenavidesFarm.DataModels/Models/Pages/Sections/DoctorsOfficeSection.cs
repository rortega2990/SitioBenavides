using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    public class DoctorsOfficeSection
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SectionMessageText { get; set; }
        public string ImageFileName { get; set; }
        public string BackgroundColor { get; set; }
        public string LogoImageFileName { get; set; }
        public string Link { get; set; }
        public string TitleColor { get; set; }
        public string SectionMessageTextColor { get; set; }
    }
}
