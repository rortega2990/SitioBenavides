using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenFarms.MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BenavidesFarm.DataModels.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BenFarms.MVC.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private MyApplicationUserManager _userManager;
        private MyApplicationSignInManager _signInManager;
        private readonly MyApplicationDbContext storeDB = new MyApplicationDbContext();

        public PerfilController()
        {
        }

        public PerfilController(MyApplicationUserManager userManager, MyApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public MyApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<MyApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public MyApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<MyApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        // GET: Perfil        
        public async Task<ActionResult> Index()
        {
            RegisterViewModel regModel = await GetPerfilViewModelUserAuthenticate();
            return View(regModel);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact()
        {
            var response = new AjaxResponse { Success = false };
            try
            {
                if (ModelState.IsValid)
                {
                    var httpRequest = System.Web.HttpContext.Current.Request;
                    var subject = string.Empty;
                    var comments = string.Empty;
                    var email = string.Empty;

                    foreach (var form in httpRequest.Form.AllKeys)
                    {
                        var formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;

                        switch (form)
                        {
                            case "Subject":
                                subject = formValue;
                                break;
                            case "Comments":
                                comments = formValue;
                                break;
                            case "Email":
                                email = formValue;
                                break;

                        }
                    }

                    bool mailSent = SendMail(email, subject, comments);
                    if (mailSent == false)
                    {
                        response.Message = "Ocurrió un error al enviar el formulario de contacto";
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }

                    response.Success = true;
                    response.Message = "Se ha enviado el mensaje satisfactoriamente";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                response.Message = "Los datos no son válidos";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                response.Message = "Ha ocurrido un error interno en el servidor -";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        private bool SendMail(string email, string subject, string comments)
        {

            var configuration = storeDB.MailConfigurations.Where(mc => mc.Active == true).FirstOrDefault();

            if (configuration == null)
            {
                return false;
            }

            string from = configuration.UserName;
            string server = configuration.Server;
            string username = configuration.UserName;
            string password = configuration.Password;
            const string to = "contactobi@benavides.com.mx";



            MailMessage message = new MailMessage();

            message.To.Add(to);
            message.From = new MailAddress(from);

            //Poner el asunto del mensaje aquí
            message.Subject = subject;

            //Poner el cuerpo del mensaje aquí
            message.Body = "<strong>Correo Electrónico:</strong> " + email + "<br /><strong>Comentarios: </strong>" + comments;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(server);
            if (configuration.Port > 0)
            {
                client.Port = configuration.Port;
            }

            //Descomentar la línea de abajo si el servidor usa un certificado que no ha sido emitido por una autoridad reconocida
            //ServicePointManager.ServerCertificateValidationCallback = (obj, certificate, chain, sslPolicyErrors) => true;

            client.Credentials = new NetworkCredential(username, password);


            if (configuration.EnableSSL == true)
            {
                client.EnableSsl = true;
            }

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateSusc(RegisterViewModel model)
        {
            var response = new AjaxResponse { Success = false };
            try
            {
                //if (ModelState.IsValid)
                //{
                    var card = SignInManager.AuthenticationManager.User.Identity.GetUserId();
                var service = new BeneficioInteligenteService.PortalBI();
                var datosCliente = service.CancelaSuscripcion("1", "998877", "1", card,model.AceptaContactoCorreo, model.AceptaContactoCorreo, model.AceptaContactoSMS, model.AceptaContactoSMS);

                if (datosCliente.ErrorCode == "CS003")
                    {
                    var httpRequest = System.Web.HttpContext.Current.Request;
                    response.Success = true;
                    var Respuesta = httpRequest.Form.GetValues("AceptaContactoCorreo");
                    response.Message = "Success";
                    //Session.Add("imageUser", user.UserProfileInfo.UserImagePerfil);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.Message = "Ocurrio un error durante la petición..";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                //}
                //else {
                //    var httpRequest = System.Web.HttpContext.Current.Request;
                ////    response.Success = true;
                //    response.Message = "Hola Mi compa";
                //    return Json(response, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception)
            {
                response.Message = "Ha ocurrido un error interno en el servidor";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Update(RegisterViewModel model)
        {
            var response = new AjaxResponse { Success = false };
            try
            {
                if (ModelState.IsValid)
                {
                    var httpRequest = System.Web.HttpContext.Current.Request;
                    var f = httpRequest.Files;

                    var image = "";
                    var dir = "~/Content/rsc/Users";
                    if (httpRequest.Files.Count > 0)
                    {
                        var postedFile = httpRequest.Files["ImagenUsuarioRegistro"];
                        var valid = Utils.IsValidImage(postedFile, FileType.Image);

                        if (valid.Value == "Ok")
                        {
                            if (postedFile != null)
                            {
                                var fil = $"{Path.GetFileNameWithoutExtension(postedFile.FileName)}_{DateTime.Now.Ticks}{postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'))}";
                                var fileSavePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(dir), fil);
                                if (!postedFile.FileName.StartsWith("/"))
                                {
                                    postedFile.SaveAs(fileSavePath);
                                    image = $"{dir}/{fil}";
                                }
                            }
                        }
                    }

                    var card = SignInManager.AuthenticationManager.User.Identity.GetUserId();
                    var service = new BeneficioInteligenteService.PortalBI();
                    var pass = service.EncriptarPassword(model.Password);
                    var birthdayArray = model.BirthDate.Split('/');
                    var birthday = birthdayArray[2]   +  birthdayArray[1] + birthdayArray[0];
                    var userHasChildren = model.HasChildren == YesNo.Sí;
                    var userClubPeques = model.ClubPeques == YesNo.Sí;
                    int sexo = model.CGenre == Genre.Female ? 2 : 1;
                    var cell = !string.IsNullOrEmpty(model.PhoneNumber) ? model.PhoneNumber.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty) : string.Empty;
                    var result = service.ActualizaDatos("1", "998877", "1", card, model.Names, model.LastName1, model.LastName2,
                        model.Email, cell, sexo, userHasChildren, userClubPeques, birthday, null, cell, image);

                    if (result.ErrorCode != "AD9902")
                    {
                        response.Message = result.Error;
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }

                    response.Success = true;
                    response.Message = "Success";
                    //Session.Add("imageUser", user.UserProfileInfo.UserImagePerfil);
                    HttpCookie aCookie = new HttpCookie("imageUser")
                    {
                        Value = string.IsNullOrEmpty(image) ? model.CGenre == Genre.Female ? "~/Content/rsc/imgs/girl.png" : "~/Content/rsc/imgs/boy.png" : image,
                        Expires = DateTime.Now.AddDays(1)
                    };
                    Response.Cookies.Add(aCookie);
                    return Json(response, JsonRequestBehavior.AllowGet);

                }
                response.Message = "Los datos no son válidos";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                response.Message = "Ha ocurrido un error interno en el servidor";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<RegisterViewModel> GetPerfilViewModelUserAuthenticate()
        {
            var card = SignInManager.AuthenticationManager.User.Identity.GetUserId();
            var service = new BeneficioInteligenteService.PortalBI();
            var datosCliente = service.ObtenerDatosCliente("1", "998877", "1", card);
            //var user = await UserManager.FindByNameAsync(userId);
            RegisterViewModel regModel = new RegisterViewModel();
            if (datosCliente != null)
            {
                //var perfil = user.UserProfileInfo;

                regModel.UserId = 1;
 
                regModel.Names = datosCliente.ReturnValue.Nombre;

                regModel.Email = datosCliente.ReturnValue.Correo;


                regModel.AceptaContactoCorreo = datosCliente.ReturnValue.FlagCorreoBI;
                regModel.AceptaContactoSMS = datosCliente.ReturnValue.FlagSMSBI;

                regModel.Card = card;

                regModel.LastName1 = datosCliente.ReturnValue.ApPaterno;
                regModel.LastName2 = datosCliente.ReturnValue.ApMaterno;

                regModel.CGenre = datosCliente.ReturnValue.Genero == 2 ? Genre.Female : Genre.Male;


                if (datosCliente.ReturnValue.FechaNacimiento != null)
                {
                    var fecha = datosCliente.ReturnValue.FechaNacimiento;
                    //10/2/2010
                    var birthday = fecha.Substring(0, 10);
                    var Month = "00" + birthday.Replace('-', '/').Split('/')[0];
                    var Day = "00" + birthday.Replace('-', '/').Split('/')[1];
                    var  Year = birthday.Replace('-', '/').Split('/')[2];
                    var Dia = Day.Substring(Day.Length - 2);
                    var Mes = Month.Substring(Month.Length - 2);
                    regModel.BirthDate =  Dia + '/' +  Mes  + '/' +  Year.Substring(0, 4);
                }

                regModel.HasChildren = datosCliente.ReturnValue.TieneHijos ? YesNo.Sí : YesNo.No;

                regModel.CodePostal = "11111";

                regModel.City = "asa";

                regModel.PhoneNumber = datosCliente.ReturnValue.TelMovil;

                var i = datosCliente.ReturnValue.ImgTitular.Length;

                var image = datosCliente.ReturnValue.ImgTitular.TrimEnd('"');

                regModel.UserImage = string.IsNullOrEmpty(image)
                    ? regModel.CGenre == Genre.Female ? "~/Content/rsc/imgs/girl.png" : "~/Content/rsc/imgs/boy.png"
                    : image;

                regModel.Mount = float.Parse(datosCliente.ReturnValue.Tarjeta_intMontoAcumulados, NumberStyles.Currency);

                regModel.CreationDateClubPeques = "-";
            }

            //var misBeneficiarioos = new List<MisBeneficiarios>();
            //misBeneficiarioos.Add(new Models.MisBeneficiarios { Nombre = "Rogelio", AppPat = "Ortega", AppMat = "Castro", FecNac = "29/06/1990", CorreoElec = "rortega@benavides.com.mx", Genero = "Masculino", Parentesco="Hermano", Telefono = "8185234532" });
            //misBeneficiarioos.Add(new Models.MisBeneficiarios { Nombre = "Francisco", AppPat = "Ortega", AppMat = "Castro", FecNac = "12/03/1989", CorreoElec = "fer10c@hotmail.com", Genero = "Masculino", Parentesco = "Hermano", Telefono = "8189994532" });
            ////public  misBeneficiarioos = new List<MisBeneficiarios>;
            //regModel.misBene = misBeneficiarioos;

            ViewBag.TuSaludURL = "";

            regModel.ConditionsTermsPage = await storeDB.ConditionsTermsPages.FirstOrDefaultAsync(x => x.ConditionsTermsPageActive);
            return regModel;
        }



        public string DeleteData(int id)
        {
            //try
            //{
            //    var company = DataRepository.GetCompanies().FirstOrDefault(c => c.ID == id);
            //    if (company == null)
            //        return "Company cannot be found";
            //    DataRepository.GetCompanies().Remove(company);
            //    return "ok";
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //}
            return null;
        }


        /// <summary>
        /// Action that updates data
        /// </summary>
        /// <param name="id">Id of the record</param>
        /// <param name="value">Value that shoudl be set</param>
        /// <param name="rowId">Id of the row</param>
        /// <param name="columnPosition">Position of the column(hidden columns are not counted)</param>
        /// <param name="columnId">Position of the column(hidden columns are counted)</param>
        /// <param name="columnName">Name of the column</param>
        /// <returns>value if update suceed - any other value will be considered as an error message on the client-side</returns>
        public string UpdateData(int id, string value, int? rowId, int? columnPosition, int? columnId, string columnName)
        {
            //var companies = DataRepository.GetCompanies();

            if (value.Length < 3)
            {
                return "No puede ser menor a 3 caractéres";
            }


            //  if (columnPosition == 0 && companies.Any(c => c.Name.ToLower().Equals(value.ToLower())))
            //return "Company with a name '" + value + "' already exists";
            //var company = companies.FirstOrDefault(c => c.ID == id);
            //if (company == null)
            //{
            //    return "Company with an id = " + id + " does not exists";
            //}
            //switch (columnPosition)
            //{
            //    case 0:
            //        company.Name = value;
            //        break;
            //    case 1:
            //        company.Address = value;
            //        break;
            //    case 2:
            //        company.Town = value;
            //        break;
            //    default:
            //        break;
            //}
            return value;
        }

        public int AddData(string name, string AppPat, string AppMat, string Parentesco, string Correo,  string Genero, string FecNac)
        {
            //var companies = DataRepository.GetCompanies();
            //if (companies.Any(c => c.Name.ToLower().Equals(name.ToLower())))
            //{
            //    Response.Write("Company with the name '" + name + "' already exists");
            //    Response.StatusCode = 404;
            //    Response.End();
            //    return -1;
            //}

            //var company = new Company();
            //company.Name = name;
            //company.Address = address;
            //company.Town = town;
            //companies.Add(company);
            //return company.ID;
            return 18;
        }



        public void IDURL(string url)
        {
            Response.Redirect(url, true);
        }



    }
}