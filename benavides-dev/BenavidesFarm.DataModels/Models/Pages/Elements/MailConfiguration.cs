using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenavidesFarm.DataModels.Models.Pages.Elements
{
    public class MailConfiguration
    {
        public int Id { get; set; }
        public int Port { get; set; }
        [Required(ErrorMessage ="Debe especificar un nombre de servidor")]
        public string Server { get; set; }
        public bool EnableSSL { get; set; }
        [Required(ErrorMessage ="Debe especificar un nombre de usuario")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Debe especificar una contraseña")]
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
    }
}
