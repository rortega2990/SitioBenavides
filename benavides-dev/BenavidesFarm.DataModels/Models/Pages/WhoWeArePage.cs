using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenavidesFarm.DataModels.Models.Pages.Sections;

namespace BenavidesFarm.DataModels.Models.Pages
{
    [Serializable]
    public class WhoWeArePage
    {
        public WhoWeArePage()
        {
            /*WhoWeAreSection = new WhoWeAreSimpleRowItem();
            VisionSection = new WhoWeAreSimpleRowItem();
            MisionSection = new WhoWeAreSimpleRowItem();
            AdSection = new WhoWeAreSimpleRowItemWithImage();
            HistoryImages = new List<WhoWeAreTitledSection>();
            HeadImages = new List<WhoWeAreTitledSection>();
            PillarsSection = new List<WhoWeAreTitledSection>();
            ValuesSection = new List<WhoWeAreTitledSection>();*/

            CreationDate = DateTime.Now;
        }
        public int Id { get; set; }
        public bool Active { get; set; }
        public WhoWeAreSimpleRowItem WhoWeAreSection { get; set; }
        public WhoWeAreSimpleRowItem VisionSection { get; set; }
        public WhoWeAreSimpleRowItem MisionSection { get; set; }
        public WhoWeAreSimpleRowItemWithImage AdSection { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual List<WhoWeAreTitledSection> HistoryImages { get; set; }
        public virtual List<WhoWeAreTitledSection> HeadImages { get; set; }
        public virtual List<WhoWeAreTitledSection> ValuesSection { get; set; }
        public virtual List<WhoWeAreTitledSection> PillarsSection { get; set; }
        public override bool Equals(object obj)
        {
            WhoWeArePage other = obj as WhoWeArePage;
            if(other != null && WhoWeAreSection.Equals(other.WhoWeAreSection) &&
                VisionSection.Equals(other.VisionSection) &&
                MisionSection.Equals(other.MisionSection) &&
                CompareTwoTitlesSections(HistoryImages, other.HistoryImages) &&
                CompareTwoTitlesSections(HeadImages, other.HeadImages) &&
                CompareTwoTitlesSections(ValuesSection, other.ValuesSection) /*&&
                CompareTwoTitlesSections(PillarsSection, other.PillarsSection)*/)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private bool CompareTwoTitlesSections(List<WhoWeAreTitledSection> current, List<WhoWeAreTitledSection> other)
        {
            return current.TrueForAll(l => other.Any(li => li.Equals(l))) && current.Count == other.Count;
        }
    }
}
