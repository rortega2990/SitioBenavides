using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BenFarms.MVC.Models
{
    public class TicketViewModel
    {
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Sucursal { get; set; }
        public string Ticket { get; set; }
        public int TipoMovimiento { get; set; }
        public decimal Importe { get; set; }
        public decimal DineroElectronico { get; set; }

        public string Producto { get; set; }

    }
}