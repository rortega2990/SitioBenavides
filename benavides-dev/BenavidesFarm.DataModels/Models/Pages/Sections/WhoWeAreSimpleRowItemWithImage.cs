using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    [Serializable]
    public class WhoWeAreSimpleRowItemWithImage: ISectionWithImage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleTextColor { get; set; }
        public string BackgroundColor { get; set; }
        public string Message { get; set; }
        public string MessageTextColor { get; set; }
        public string ImageFileName { get; set; }
    }
}
