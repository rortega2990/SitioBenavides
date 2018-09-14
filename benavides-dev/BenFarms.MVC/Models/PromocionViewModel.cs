using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Models
{
    public class PromocionViewModel
    {
        public string Promocion { get; set; }
        public string Cupon { get; set; }
        public string Vigencia { get; set; }

    }

    


    public class BirthdayViewModel
    {
        public string Tarjeta { get; set; }
        public string Codigo{ get; set; }
        public string Respuesta { get; set; }

        public BirthdayViewModel(string Tarjeta, string Codigo, string Respuesta)
        {
            this.Tarjeta = Tarjeta;
            this.Codigo = Codigo;
            this.Respuesta = Respuesta;
        }
    }


}