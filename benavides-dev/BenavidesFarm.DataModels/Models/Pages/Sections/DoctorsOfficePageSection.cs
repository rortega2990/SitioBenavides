using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    public class DoctorsOfficePageSection:ISectionWithImage, IComparable<DoctorsOfficePageSection>
    {
        public int Id { get; set; }
        public string ImageFileName { get; set; }
        public string Title { get; set; }
        public string TitleColor { get; set; }

        public int CompareTo(DoctorsOfficePageSection other)
        {
            return Title.CompareTo(other.Title);
        }

        public override bool Equals(object obj)
        {
            var other = obj as DoctorsOfficePageSection;

            if (other != null && Title == other.Title &&
                TitleColor == other.TitleColor &&
                (ImageFileName == other.ImageFileName || ImageFileName == other.Id.ToString()))
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
