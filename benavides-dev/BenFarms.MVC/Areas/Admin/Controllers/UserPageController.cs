using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BenavidesFarm.DataModels.Models;
using System.Data.Entity;
using System.Net;
using System;
using BenFarms.MVC.Models;
using System.Collections.Generic;
using System.IO;
using Microsoft.Owin.Security;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserPageController : Controller
    {
        private readonly MyApplicationDbContext db = new MyApplicationDbContext();
        private MyApplicationUserManager _userManager;
        private MyApplicationRoleManager _roleManager;
        private MyApplicationSignInManager _signInManager;

        public UserPageController()
        {
        }

        public UserPageController(MyApplicationUserManager userManager, MyApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
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

        public MyApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<MyApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public MyApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<MyApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public async Task<ActionResult> Index()
        {
            var users = await db.Users.Where(x => x.TypeUser == "WebAdmin").ToListAsync();
            List<RegisterViewModel> reg = users.Select(GetRegisterViewModel).ToList();
            return View(reg);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await db.Users.FirstAsync(x => x.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(GetRegisterViewModel(user));
        }

        public async Task<ActionResult> DetailsRegUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await db.Users.FirstAsync(x => x.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(GetRegisterViewModel(user));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            var response = new AjaxResponse { Success = false };
            try
            {
                if (ModelState.IsValid)
                {
                    var user = UserManager.FindByName(model.Email);
                    if (user == null)
                    {
                        const string roleName = "Admin";
                        IdentityResult resultRole = IdentityResult.Success;
                        var role = RoleManager.FindByName(roleName);
                        if (role == null)
                        {
                            role = new MyApplicationRole(roleName, "Role Admin");
                            resultRole = RoleManager.Create(role);
                        }

                        DateTime? fecha = null;
                        if (!string.IsNullOrEmpty(model.BirthDate))
                        {
                            try
                            {
                                fecha = DateTime.ParseExact(model.BirthDate, "dd/MM/yyyy", null);
                            }catch
                            {
                                response.Message = "La fecha no es válida.";
                                return Json(response, JsonRequestBehavior.AllowGet);
                            }
                        }

                        DateTime dateC = DateTime.Now;
                        user = new MyApplicationUser
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            CardUser = Guid.NewGuid().ToString(),
                            PhoneNumber = model.PhoneNumber,
                            TypeUser = "WebAdmin",
                            UserProfileInfo = new UserProfileInfo
                            {
                                UserNames = model.Names,
                                UserLastName1 = model.LastName1,
                                UserLastName2 = model.LastName2,
                                UserBirthDate = fecha.Value,
                                UserFemale = model.CGenre == Genre.Female,
                                UserHasChildren = model.HasChildren == YesNo.Sí,
                                UserClubPeques = model.ClubPeques == YesNo.Sí,
                                UserMount = model.Mount,
                                UserCreationDate = dateC,
                                UserUpdateDate = dateC,
                                UserCreationDateClubPeques = dateC,
                                UserCity = model.City,
                                UserCodePostal = model.CodePostal,
                            }
                        };

                        IdentityResult resultUser = await UserManager.CreateAsync(user, model.Password);

                        if (resultUser == IdentityResult.Success && resultRole == IdentityResult.Success)
                        {
                            UserManager.SetLockoutEnabled(user.Id, false);

                            // Add user admin to Role Admin if not already added
                            var rolesForUser = UserManager.GetRoles(user.Id);
                            if (!rolesForUser.Contains(role.Name))
                            {
                                UserManager.AddToRole(user.Id, role.Name);
                                response.Success = true;
                                response.Message = "La cuenta de usuario se creó satisfactoriamente.";
                                return Json(response, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        response.Message = "Ya existe la dirección de correo, introduzca otra.";
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                }
                response.Message = "Los datos no son válidos";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                response.Message = "Ha ocurrido un error al intentar crear la cuenta de usuario.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> EditProfile(int? id)
        {
            var id1 = id;
            if (id == null)
            {
                if(User.Identity.IsAuthenticated)
                {
                    var d = User.Identity.GetUserId();
                    id1 = int.Parse(d);
                }else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }                
            }
            var user = await db.Users.FirstAsync(x => x.Id == id1);
            if (user == null)
            {
                return HttpNotFound();
            }
            if(user.Id == int.Parse(User.Identity.GetUserId()))
            {
                return View(GetRegisterViewModel(user));
            }
            return View("Update", GetRegisterViewModel(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditInfo([Bind(Include = "UserId,Names,LastName1,LastName2,BirthDate,CGenre,PhoneNumber,CodePostal,City")] RegisterViewModel model)
        {
            var id1 = model.UserId;
            if (id1 == 0)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var d = User.Identity.GetUserId();
                    id1 = int.Parse(d);
                }
            }
            var user = UserManager.FindById(id1);
            if (user == null)
            {
                return HttpNotFound();
            }
            try
            {
                DateTime? fecha = null;
                if (!string.IsNullOrEmpty(model.BirthDate))
                {
                    try
                    {
                        fecha = DateTime.ParseExact(model.BirthDate, "dd/MM/yyyy", null);
                    }
                    catch
                    {
                        return Json(new AjaxResponse { Success = false, Message = "La fecha no es válida." }, JsonRequestBehavior.AllowGet);
                    }
                }

                GetUserViewModel(model, user, fecha.Value);
                await UserManager.UpdateAsync(user);
                return Json(new AjaxResponse { Success = true, Message = "Los datos se actualizaron satisfactoriamente." }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new AjaxResponse { Success = false, Message = "Ha ocurrido un error al intentar actualizar los datos." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditInfo1([Bind(Include = "UserId,Names,LastName1,LastName2,BirthDate,CGenre,Email,PhoneNumber,CodePostal,City,Password,ConfirmPassword")] RegisterViewModel model)
        {
            var id1 = model.UserId;
            if (id1 == 0)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var d = User.Identity.GetUserId();
                    id1 = int.Parse(d);
                }
            }
            var user = UserManager.FindById(id1);
            if (user == null)
            {
                return HttpNotFound();
            }
            try
            {
                DateTime? fecha = null;
                if (!string.IsNullOrEmpty(model.BirthDate))
                {
                    try
                    {
                        fecha = DateTime.ParseExact(model.BirthDate, "dd/MM/yyyy", null);
                    }
                    catch
                    {
                        return Json(new AjaxResponse { Success = false, Message = "La fecha no es válida." }, JsonRequestBehavior.AllowGet);
                    }
                }

                var userEmail = await UserManager.FindByEmailAsync(model.Email);
                if (userEmail == null || userEmail.Id == user.Id)
                {
                    GetUserViewModel(model, user, fecha.Value);
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    await UserManager.UpdateAsync(user);
                    var result = await UserManager.UpdatePassword(user, model.Password);
                    return Json(new AjaxResponse { Success = true, Message = "Los datos se actualizaron satisfactoriamente." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new AjaxResponse { Success = false, Message = "Ya existe la dirección de correo, no se puede actualizar." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new AjaxResponse { Success = false, Message = "Ha ocurrido un error al intentar actualizar los datos." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> EditImage(int? id)
        {
            var httpRequest = System.Web.HttpContext.Current.Request;
            var id1 = 0;
            if (httpRequest.Form.AllKeys.Length > 0)
            {
                var formValue = httpRequest.Form[httpRequest.Form.AllKeys[0]];
                if (!string.IsNullOrEmpty(formValue))
                {
                    id1 = int.Parse(formValue);
                }
            }
            
            if (id1 == 0)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var d = User.Identity.GetUserId();
                    id1 = int.Parse(d);
                }
            }
            var user = UserManager.FindById(id1); ;
            if (user == null)
            {
                return HttpNotFound();
            }

            try
            {
                var dir = "~/Content/rsc/Users";
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
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
                                user.UserProfileInfo.UserImagePerfil = $"{dir}/{fil}";
                                await UserManager.UpdateAsync(user);

                                if (Response.Cookies["imageUser"] != null)
                                {
                                    Response.Cookies.Remove("imageUser");
                                }
                                HttpCookie aCookie = new HttpCookie("imageUser")
                                {
                                    Value = string.IsNullOrEmpty(user.UserProfileInfo.UserImagePerfil) ? user.UserProfileInfo.UserFemale ? "~/Content/rsc/imgs/girl.png" : "~/Content/rsc/imgs/boy.png" : user.UserProfileInfo.UserImagePerfil,
                                    Expires = DateTime.Now.AddDays(1)
                                };
                                Response.Cookies.Add(aCookie);

                                return Json(new AjaxResponse { Success = true, Data = aCookie.Value.Substring(1),  Message = "La imagen de perfil se actualizó satisfactoriamente." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        return Json(new AjaxResponse { Success = false, Message = valid.Value }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch(Exception)
            {
                return Json(new AjaxResponse { Success = false, Message = "Ha ocurrido un error al intentar subir la imagen." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new AjaxResponse { Success = false, Message = "Ha ocurrido un error al intentar subir la imagen." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPass([Bind(Include = "Password,NewPassword,ConfirmPassword")] RegisterViewModel model)
        {
            var id1 = model.UserId;
            if (id1 == 0)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var d = User.Identity.GetUserId();
                    id1 = int.Parse(d);
                }
            }
            var user = UserManager.FindById(id1); ;
            if (user == null)
            {
                return HttpNotFound();
            }

            try
            {
                var resultPass = UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.Password);
                if (resultPass == PasswordVerificationResult.Success)
                {
                    var result = await UserManager.UpdatePassword(user, model.NewPassword);

                    if (result == IdentityResult.Success)
                    {
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        await SignInManager.SignInAsync(user, true, true);
                        return Json(new AjaxResponse { Success = true, Message = "La contraseña se actualizó satisfactoriamente." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new AjaxResponse { Success = false, Message = "La contraseña anterior no es correcta." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new AjaxResponse { Success = false, Message = "Ha ocurrido un error al intentar actualizar la contraseña." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new AjaxResponse { Success = false, Message = "Ha ocurrido un error al intentar actualizar la contraseña." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEmail([Bind(Include = "Email,Password")] RegisterViewModel model)
        {
            var id1 = model.UserId;
            if (id1 == 0)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var d = User.Identity.GetUserId();
                    id1 = int.Parse(d);
                }
            }
            var user = UserManager.FindById(id1); ;
            if (user == null)
            {
                return HttpNotFound();
            }

            try
            {
                var userEmail = await UserManager.FindByEmailAsync(model.Email);
                if (userEmail == null || userEmail.Id == user.Id)
                {
                    var resultPass = UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.Password);
                    if (resultPass == PasswordVerificationResult.Success)
                    {
                        user.Email = model.Email;
                        user.UserName = model.Email;
                        await UserManager.UpdateAsync(user);
                        var result = await UserManager.UpdatePassword(user, model.Password);

                        if (result == IdentityResult.Success)
                        {
                            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                            await SignInManager.SignInAsync(user, true, true);
                            return Json(new AjaxResponse { Success = true, Message = "La dirección de correo se actualizó satisfactoriamente." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new AjaxResponse { Success = false, Message = "Credenciales incorrectas" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new AjaxResponse { Success = false, Message = "Ya existe la dirección de correo, no se puede actualizar." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new AjaxResponse { Success = false, Message = "Ha ocurrido un error al intentar actualizar el Email." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new AjaxResponse { Success = false, Message = "Ha ocurrido un error al intentar actualizar el Email." }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await db.Users.FirstAsync(x => x.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(GetRegisterViewModel(user));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MyApplicationUser user = await db.Users.FirstAsync(x => x.Id == id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteRegUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await db.Users.FirstAsync(x => x.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(GetRegisterViewModel(user));
        }

        [HttpPost, ActionName("DeleteRegUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteContactUserConfirmed(int id)
        {
            MyApplicationUser user = await db.Users.FirstAsync(x => x.Id == id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("UserReg");
        }

        public async Task<ActionResult> UserReg()
        {
            var users = await db.Users.Where(x => x.TypeUser == "Benavides").ToListAsync();

            List<RegisterViewModel> reg = users.Select(GetRegisterViewModel).ToList();
            return View(reg);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private RegisterViewModel GetRegisterViewModel(MyApplicationUser user)
        {
            RegisterViewModel regModel = new RegisterViewModel();
            if (user != null)
            {
                var perfil = user.UserProfileInfo;

                regModel.Names = perfil.UserNames;

                regModel.Email = user.Email;

                regModel.Card = user.CardUser;

                regModel.CreationDate = perfil.UserCreationDate;

                regModel.LastName1 = perfil.UserLastName1;

                regModel.LastName2 = perfil.UserLastName2;

                regModel.LastNames = $"{perfil.UserLastName1} {perfil.UserLastName2}";

                regModel.CGenre = perfil.UserFemale ? Genre.Female : Genre.Male;

                //regModel.BirthDate = perfil.UserBirthDate != null ?
                //    $"{perfil.UserBirthDate.Value.Month}/{perfil.UserBirthDate.Value.Day}/{perfil.UserBirthDate.Value.Year}"
                //    : null;
                regModel.BirthDate = perfil.UserBirthDate != null ?
                    convertToDate(perfil.UserBirthDate)
                    : null;

                regModel.CodePostal = perfil.UserCodePostal;

                regModel.City = perfil.UserCity;

                regModel.UserImage = perfil.UserImagePerfil;

                regModel.PhoneNumber = user.PhoneNumber;

                regModel.UserId = user.Id;
            }
            regModel.ConditionsTermsPage = db.ConditionsTermsPages.FirstOrDefault(x => x.ConditionsTermsPageActive);
            return regModel;
        }

        private string convertToDate(DateTime? fecha)
        {
            var dia = fecha.Value.Day;
            var sdia = dia.ToString();
            var mes = fecha.Value.Month;
            var smes = mes.ToString();
            var anno = fecha.Value.Year.ToString();
            if(dia.ToString().Length == 1)
            {
                sdia = "0" + sdia;
            }
            if (mes.ToString().Length == 1)
            {
                smes = "0" + smes;
            }

            return $"{sdia}/{smes}/{anno}";
        }

        private void GetUserViewModel(RegisterViewModel regModel, MyApplicationUser myAppUser, DateTime fecha)
        {
            if (regModel != null)
            {                

                myAppUser.UserProfileInfo.UserNames = regModel.Names;

                //myAppUser.Email = regModel.Email;

                //myAppUser.CardUser = regModel.Card;

                myAppUser.TypeUser = "WebAdmin";

                //myAppUser.UserProfileInfo.CreationDate = regModel.CreationDate;

                myAppUser.UserProfileInfo.UserLastName1 = regModel.LastName1;

                myAppUser.UserProfileInfo.UserLastName2 = regModel.LastName2;

                myAppUser.UserProfileInfo.UserFemale = regModel.CGenre == Genre.Female;

                myAppUser.UserProfileInfo.UserBirthDate = fecha;

                myAppUser.UserProfileInfo.UserCodePostal = regModel.CodePostal;

                myAppUser.UserProfileInfo.UserCity = regModel.City;

                //myAppUser.UserProfileInfo.UserImagePerfil = regModel.UserImage;

                myAppUser.PhoneNumber = regModel.PhoneNumber;
            }
        }
    }
}