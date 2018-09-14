using BenavidesFarm.DataModels.Models.Pages.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenavidesFarm.DataModels.Models.Pages
{
    public class DoctorsOfficePage
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public DateTime CreationDate { get; set; }
        public List<DoctorsOfficePageSection> HeadImages { get; set; }
        public List<DoctorsOfficePageSection> ServicesSection { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as DoctorsOfficePage;
            if(other != null && CompareTwoSections(HeadImages, other.HeadImages) == false &&
                CompareTwoSections(ServicesSection, other.ServicesSection) == false)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private bool CompareTwoSections(List<DoctorsOfficePageSection> current, List<DoctorsOfficePageSection> other)
        {
            return current.TrueForAll(l => other.Any(li => li.Equals(l))) && current.Count == other.Count;
        }
    }
}
