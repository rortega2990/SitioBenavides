using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{

    public class Laboratory
    {
        private string name;
        public Laboratory()
        {
        }
        public Municipios City { get; set; }
        public Estados State { get; set; }
        public String Name {
            get {
                return this.name;
            }
            set {
                if(value.Trim().Length == 0)
                {
                    throw new ArgumentException("Invalid laboratory name provided");
                }
                this.name = value;
            }
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public bool Active { get; set; }
    }
}
