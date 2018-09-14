using BenavidesFarm.DataModels.Models.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenFarms.MVC.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Tarjeta:")]
        public string CardLogin { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña:")]
        public string PasswordLogin { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }
    }

    //nombre_Beneficiario1>string</nombre_Beneficiario1>
    //  <apellidoPaterno_Beneficiario1>string</apellidoPaterno_Beneficiario1>
    //  <apellidoMaterno_Beneficiario1>string</apellidoMaterno_Beneficiario1>
    //  <idparentezco_Beneficiario1>int</idparentezco_Beneficiario1>
    //  <fechaNacimiento_Beneficiario1>string</fechaNacimiento_Beneficiario1>
    //  <email_Beneficiario1>string</email_Beneficiario1>
    //  <id_sexo_Beneficiario1>int</id_sexo_Beneficiario1>

    public class MisBeneficiarios
    {
        static int nextID = 17;

        public MisBeneficiarios()
        {
            ID = nextID++;
        }
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string CorreoElec { get; set; }
        public string AppMat { get; set; }
        public string AppPat { get; set; }
        public string FecNac { get; set; }
        public string Genero { get; set; }
        public string Parentesco { get; set; }
    }

    public class RegisterViewModel
    {
        public int UserId { get; set; }

        //[Required(ErrorMessage = "El campo Tarjeta es requerido")]
        //[StringLength(20, ErrorMessage = "Longitud permitida para el campo Tarjeta es 20")]
        //[DataType(DataType.CreditCard)]
        [Display(Name = "*Tarjeta:")]
        public string Card { get; set; }

        //[Required(ErrorMessage = "El campo Nombre(s) es requerido")]
        //[MaxLength(100, ErrorMessage = "Longitud máxima del campo Nombre(s) es 100"), MinLength(2, ErrorMessage = "Longitud mínima del campo Nombre(s) es 2")]
        [Display(Name = "*Nombre(s):")]
        public string Names { get; set; }

        //[Required(ErrorMessage = "El campo Apellido paterno es requerido")]
        //[MaxLength(100, ErrorMessage = "Longitud máxima del campo Apellido paterno es 100"), MinLength(2, ErrorMessage = "Longitud mínima del campo Apellido paterno es 2")]
        [Display(Name = "*Apellido paterno:")]
        public string LastName1 { get; set; }

        //[Required(ErrorMessage = "El campo Apellido materno es requerido")]
        //[MaxLength(100, ErrorMessage = "Longitud máxima del campo Apellido materno es 100"), MinLength(2, ErrorMessage = "Longitud mínima del campo Apellido materno es 2")]
        [Display(Name = "*Apellido materno:")]
        public string LastName2 { get; set; }

        [Display(Name = "Apellidos:")]
        public string LastNames { get; set; }

        //[Required(ErrorMessage = "El campo Fecha de nacimiento es requerido")]
        //[StringLength(10, ErrorMessage = "Longitud permitida para el campo Fecha de nacimiento es 10")]
        [Display(Name = "*Fecha de nacimiento:")]
        public string BirthDate { get; set; }

        //[Required(ErrorMessage = "El campo Género es requerido")]
        [Display(Name = "*Género")]
        public Genre CGenre { get; set; }

        //[Required(ErrorMessage = "El campo ¿Tienes hijos? es requerido")]
        [Display(Name = "*¿Tienes hijos?")]
        public YesNo HasChildren { get; set; }

        [Display(Name = "¿Quieres pertenecer a?")]
        public YesNo? ClubPeques { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Display(Name = "Celular (10 dígitos):")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "El campo E-mail es requerido")]
        [EmailAddress(ErrorMessage = "El campo E-mail no tiene un formato de correo electrónico correcto")]
        [Display(Name = "*E-mail:")]
        public string Email { get; set; }


        [Required(ErrorMessage = "El campo AceptaContacto es requerido")]
        [Display(Name = "AceptaContactoCorreo:")]
        public bool  AceptaContactoCorreo { get; set; }

        [Required(ErrorMessage = "El campo AceptaContactoSMS es requerido")]
        [Display(Name = "AceptaContactoSMS:")]
        public bool AceptaContactoSMS { get; set; }



        //[Required(ErrorMessage = "El campo Contraseña es requerido")]
        [MaxLength(100, ErrorMessage = "Longitud máxima del campo Contraseña es 50"), MinLength(2, ErrorMessage = "Longitud mínima del campo Contraseña es 6")]
        [DataType(DataType.Password)]
        [Display(Name = "*Contraseña:")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "El campo Confirmar contraseña es requerido")]
        [MaxLength(100, ErrorMessage = "Longitud máxima del campo Confirmar contraseña es 50"), MinLength(2, ErrorMessage = "Longitud mínima del campo Confirmar contraseña es 6")]
        [DataType(DataType.Password)]
        [Display(Name = "*Confirmar contraseña:")]
        [Compare("Password", ErrorMessage = "Los valores de los campos Contraseña y Confirmar contraseña no coinciden")]
        public string ConfirmPassword { get; set; }

        [MaxLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "*Nueva Contraseña:")]
        public string NewPassword { get; set; }

        [Display(Name = "Código Postal:")]
        public string CodePostal { get; set; }

        [Display(Name = "Ciudad:")]
        public string City { get; set; }

        [Display(Name = "Fecha de Creación:")]
        public DateTime CreationDate { get; set; }

        public string UserImage { get; set; }

        //public IList<MisBeneficiarios> misBene { get; set; }

        public float Mount { get; set; }

        public string CreationDateClubPeques { get; set; }

        [Display(Name = "*Género")]
        public string GenreSex => CGenre == Genre.Female ? "Femenino" : "Masculino";

        public ConditionsTermsPage ConditionsTermsPage { get; set; }
    }

    public class AjaxResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }    

    public enum Genre { Male = 1, Female = 2, }

    //public  Beneficiarios {     }

    public enum YesNo { Sí = 1, No = 2 }

    public class ResetPasswordViewModel
    {
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña Anterior: ")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña: ")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña: ")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Tarjeta:")]
        public string Card { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
