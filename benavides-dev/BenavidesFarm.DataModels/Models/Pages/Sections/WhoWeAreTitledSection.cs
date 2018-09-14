using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    [Serializable]
    public class WhoWeAreTitledSection: IComparable<WhoWeAreTitledSection>, ISectionWithImage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TileColor { get; set; }
        public string SubTitle { get; set;}
        public string SubTitleColor { get; set; }
        public string Text { get; set; }
        public string TextColor { get; set; }
        public string ImageFileName { get; set; }

        public int CompareTo(WhoWeAreTitledSection other)
        {
            return Title.CompareTo(other.Title);
        }

        public override bool Equals(object obj)
        {
            var other = obj as WhoWeAreTitledSection;
            if(other != null && Title == other.Title &&
                TileColor == other.TileColor &&
                SubTitle == other.SubTitle &&
                SubTitleColor == other.SubTitleColor &&
                Text == other.Text &&
                TextColor == other.TextColor &&
                (ImageFileName == other.ImageFileName || ImageFileName == other.Id.ToString()) )
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
