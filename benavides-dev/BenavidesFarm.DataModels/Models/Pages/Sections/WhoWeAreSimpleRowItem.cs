using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    [Serializable]
    public class WhoWeAreSimpleRowItem
    {
        public string Title { get; set; }
        public string TitleTextColor { get; set; }
        public string BackgroundColor { get; set; }
        public string Message { get; set; }
        public string MessageTextColor { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as WhoWeAreSimpleRowItem;
            if(other != null &&Title == other.Title && TitleTextColor == other.TitleTextColor &&
                    BackgroundColor == other.BackgroundColor &&
                    Message == other.Message &&
                    MessageTextColor == other.MessageTextColor)
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
