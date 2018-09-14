using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models;
using BenFarms.MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace BenFarms.MVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private MyApplicationUserManager _userManager;
        private MyApplicationSignInManager _signInManager;

        public AccountController()
        {
        }

        public AccountController(MyApplicationUserManager userManager, MyApplicationSignInManager signInManager)
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

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword()
        {
            var response = new AjaxResponse { Success = false };
            var errorMessages = new List<string>();
            if (!ModelState.IsValid)
            {
                errorMessages.AddRange(from key in ModelState.Values where key.Errors.Count > 0 select key.Errors[0].ErrorMessage);
                response.Message = errorMessages[0];
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                var httpRequest = System.Web.HttpContext.Current.Request;
                var card = string.Empty;
                var email = string.Empty;

                foreach (var form in httpRequest.Form.AllKeys)
                {
                    var formValue = httpRequest.Form[form];
                    formValue = string.IsNullOrEmpty(formValue) ? null : formValue;

                    switch (form)
                    {
                        case "Card":
                            card = formValue;
                            break;

                        case "Email":
                            email = formValue;
                            break;

                    }
                }

                var service = new BeneficioInteligenteService.PortalBI();
                var result = service.RecuperarPassword("1", "998877", "1", card, email);
                response.Message = result.Error;
                switch (result.ErrorCode)
                {
                    case "ECC105":
                        response.Message = "Se le ha enviado un correo con la contraseña";
                        response.Success = true;
                        break;
                    case "ECC101":
                        response.Message = "Tarjeta Inexistente";
                        response.Success = false;
                        break;
                    case "ECC102":
                        response.Message = "Tarjeta Inactiva";
                        response.Success = false;
                        break;
                    case "ECC103":
                        response.Message = "Correo Inválido";
                        response.Success = false;
                        break;
                    case "ECC104":
                        response.Message = "Datos Incorrectos";
                        response.Success = false;
                        break;
                    default:
                        response.Success = false;
                        break;
                }

            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ResetPasswordViewModel model)
        {
            var card = SignInManager.AuthenticationManager.User.Identity.GetUserId();
            var response = new AjaxResponse { Success = false };
            var errorMessages = new List<string>();
            if (!ModelState.IsValid)
            {
                errorMessages.AddRange(from key in ModelState.Values where key.Errors.Count > 0 select key.Errors[0].ErrorMessage);
                response.Message = errorMessages[0];
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                var service = new BeneficioInteligenteService.PortalBI();
                var pass = service.EncriptarPassword(model.Password);
                var oldPass = service.EncriptarPassword(model.OldPassword);

                var result = service.CambiarPassword("1", "998877", "1", card, oldPass, pass);
                response.Message = result.ErrorCode;

                switch (result.Error)
                {
                    case "CON001":
                        response.Message = "Se cambió con éxito la contraseña";
                        response.Success = true;
                        break;
                    case "CON003":
                        response.Message = "Captura correctamente la contraseña actual";
                        response.Success = false;
                        break;
                    default:
                        response.Success = false;
                        response.Message = "Ha ocurrido un error";
                        break;
                }

            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var response = new AjaxResponse { Success = false };

            var errorMessages = new List<string>();
            if (!ModelState.IsValid)
            {
                errorMessages.AddRange(from key in ModelState.Values where key.Errors.Count > 0 select key.Errors[0].ErrorMessage);
                response.Message = errorMessages[0];
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            var service = new BeneficioInteligenteService.PortalBI();
            var pass = service.EncriptarPassword(model.PasswordLogin);
            var result = service.ValidaAcceso("1", "998877", "1", model.CardLogin, pass);

            switch (result.ErrorCode)
            {
                case "0":
                    var datosCliente = service.ObtenerDatosCliente("1", "998877", "1", model.CardLogin);
                    var mount = datosCliente.ReturnValue.Tarjeta_intMontoAcumulados;
                    float montoAcumulado = float.Parse(mount, CultureInfo.InvariantCulture);

                    var ident = new ClaimsIdentity(
                          new[] { 
                              // adding following 2 claim just for supporting default antiforgery provider
                              new Claim(ClaimTypes.NameIdentifier, model.CardLogin),
                              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                              new Claim(ClaimTypes.Name,model.CardLogin),

                              // optionally you could add roles if any
                              new Claim(ClaimTypes.Role, "Benavides"),
                          },

                          "ApplicationCookie");

                    HttpContext.GetOwinContext().Authentication.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);

                    HttpCookie aCookie = new HttpCookie("imageUser")
                    {
                        Value = string.IsNullOrEmpty(datosCliente.ReturnValue.ImgTitular) ? datosCliente.ReturnValue.Genero == 2 ? "~/Content/rsc/imgs/girl.png" : "~/Content/rsc/imgs/boy.png" : datosCliente.ReturnValue.ImgTitular.TrimEnd('"'),
                        //Value = datosCliente.datosCliente.Genero == 2 ? "~/Content/rsc/imgs/girl.png" : "~/Content/rsc/imgs/boy.png",
                        Expires = DateTime.Now.AddDays(1)
                    };
                    Response.Cookies.Add(aCookie);
                    response.Message = "Success";
                    response.Success = true;
                    response.Data = new RegisterViewModel { Names = datosCliente.ReturnValue.Nombre, Card = model.CardLogin, Mount = montoAcumulado, CreationDateClubPeques = "-" };
                    break;
                default:
                    response.Message = "Credenciales inválidas";
                    break;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(int id)
        {
            RegisterViewModel model = new RegisterViewModel();

            var response = new AjaxResponse { Success = false, Message = "Operación no implementada" };
            try
            {
                if (ModelState.IsValid)
                {
                    var httpRequest = System.Web.HttpContext.Current.Request;
                    foreach (var form in httpRequest.Form.AllKeys)
                    {
                        var formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;

                        switch (form)
                        {
                            case "Card":
                                model.Card = formValue;
                                break;
                            case "Names":
                                model.Names = formValue;
                                break;
                            case "LastName1":
                                model.LastName1 = formValue;
                                break;
                            case "LastName2":
                                model.LastName2 = formValue;
                                break;
                            case "BirthDate":
                                model.BirthDate = formValue;
                                break;
                            case "CGenre":
                                model.CGenre = formValue == "1" ? Genre.Male : Genre.Female;
                                break;
                            case "HasChildren":
                                model.HasChildren = formValue == "1" ? YesNo.Sí : YesNo.No;
                                break;
                            case "ClubPeques":
                                model.ClubPeques = formValue != null && formValue == "1" ? YesNo.Sí : YesNo.No;
                                break;
                            case "Email":
                                model.Email = formValue;
                                break;
                            case "PhoneNumber":
                                model.PhoneNumber = formValue;
                                break;
                            case "Password":
                                model.Password = formValue;
                                break;
                            case "CodePostal":
                                model.CodePostal = formValue;
                                break;
                            case "City":
                                model.City = formValue;
                                break;
                        }
                    }

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

                    var cell = !string.IsNullOrEmpty(model.PhoneNumber) ? model.PhoneNumber.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty) : string.Empty;
                    var service = new BeneficioInteligenteService.PortalBI();
                    //var datosCliente = service.ObtenerDatosCliente("1", "998877", "1", model.Card);
                    var pass = service.EncriptarPassword(model.Password);
                    var birthdayArray = model.BirthDate.Split('/');
                    //var birthday = birthdayArray[2] + birthdayArray[0] + birthdayArray[1];
                    var birthday = birthdayArray[2] + birthdayArray[1] + birthdayArray[0];
                    var userHasChildren = model.HasChildren == YesNo.Sí;
                    var userClubPeques = model.ClubPeques == YesNo.Sí;
                    int sexo = model.CGenre == Genre.Female ? 2 : 1;



                    var result = service.ActivarAcceso("1", "998877", "1", model.Card, model.Names, model.LastName1,
                        model.LastName2, cell, sexo, userHasChildren, userClubPeques, birthday, null, pass, model.Email, true, true, true, true, true, cell, image);

                    if (result.ErrorCode != "0")
                    {
                        response.Message = result.Error;
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }

                    var ident = new ClaimsIdentity(
                      new[] { 
                          // adding following 2 claim just for supporting default antiforgery provider
                          new Claim(ClaimTypes.NameIdentifier, model.Card),
                          new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                          new Claim(ClaimTypes.Name,model.Card),

                          // optionally you could add roles if any
                          new Claim(ClaimTypes.Role, "Benavides"),
                      },

                      "ApplicationCookie");

                    HttpContext.GetOwinContext().Authentication.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);


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
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var response = new AjaxResponse { Success = false };
            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                if (Response.Cookies["imageUser"] != null)
                {
                    Response.Cookies.Remove("imageUser");
                }
                response.Message = "Success";
                response.Success = true;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var excep = ex.GetBaseException() as System.Data.SqlClient.SqlException;
                if (excep != null)
                {
                    response.Message = ex.GetBaseException().Message;
                    return Json(response.Message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    response.Message = ex.GetBaseException().Message;
                    return Json(response.Message, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetDataFromCard(string card)
        {
            var service = new BeneficioInteligenteService.PortalBI();

            var response = new AjaxResponse { Success = false, Message = "Operación no implementada", Data = null };
            try
            {
                var datosCliente = service.ObtenerDatosCliente("1", "998877", "1", card);
                var datos = datosCliente.ReturnValue;
                if (datos != null)
                {
                    response.Success = true;
                    string birthdayValue = string.Empty;

                    if (datos.FechaNacimiento != null)
                    {
                        var fecha = datos.FechaNacimiento;

                        var birthday = fecha.Split(' ');
                        var birthdayTmp = birthday[0].Split('/');
                        birthdayValue = birthdayTmp[1].PadLeft(2, '0') + "/" + birthdayTmp[0].PadLeft(2, '0') + "/" + birthdayTmp[2];

                    }

                    var i = datos.ImgTitular.Length;

                    var image = datos.ImgTitular.TrimEnd('"');

                    var userImage = string.IsNullOrEmpty(image) ? datos.Genero == 2 ? "~/Content/rsc/imgs/girl.png" : "~/Content/rsc/imgs/boy.png" : image;


                    response.Data = new
                    {
                        ApMaterno = datos.ApMaterno,
                        ApPaterno = datos.ApPaterno,
                        Email = datos.Correo,
                        FechaNacimiento = birthdayValue,
                        Genero = datos.Genero,
                        Imagen = @Url.Content(userImage),
                        Nombre = datos.Nombre,
                        Celular = datos.TelMovil
                    };
                    return
                        Json(response, JsonRequestBehavior.AllowGet);
                }
                response.Message = "Tarjeta Inexistente o Inactiva";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                response.Message = "Ha ocurrido un error interno en el servidor";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}