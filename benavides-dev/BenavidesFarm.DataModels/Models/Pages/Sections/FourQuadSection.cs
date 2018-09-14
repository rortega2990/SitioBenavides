using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenavidesFarm.DataModels.Models.Pages.Sections
{
    public class FourQuadSection
    {
        public int Id { get; set; }
        public SingleQuadSectionItem Quad1 { get; set; }
        public SingleQuadSectionItem Quad2 { get; set; }
        public SingleQuadSectionItem Quad3 { get; set; }
        public SingleQuadSectionItem Quad4 { get; set; }
    }
}
