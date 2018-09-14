using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Areas.Admin.Models
{
    public class ConfirmDeletionViewModel
    {
        public string Title { get; set; }
        public string Controller { get; set; }
        public DateTime CreationDate { get; set; }
        public int Id { get; set; }
        public bool Active { get; set; }
        public string CustomValue { get; set; }
    }
}