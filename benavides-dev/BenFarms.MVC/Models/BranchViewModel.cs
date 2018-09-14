using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Models
{
    public class BranchViewModel
    {
        public BranchViewModel() { }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Hour { get; set; }
    }
}