using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages;
using BenFarms.MVC.Models;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;

namespace BenFarms.MVC.Controllers
{
    public class JoinTeamController : Controller
    {
        readonly MyApplicationDbContext storeDB;

        public JoinTeamController()
        {
            storeDB = new MyApplicationDbContext();
        }

        // GET: JoinTeam
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var joinTeamPage = await GetActiveJoinTeamPage();
            if (joinTeamPage != null)
                return View(joinTeamPage);
            return RedirectToAction("NotFound", "Error");
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Join(int? id)
        {
            var response = new AjaxResponse { Success = false };
            try
            {
                
                if (ModelState.IsValid)
                {

                    if (string.IsNullOrEmpty(Request.Form["TeamInterestArea"]))
                    {
                        response.Message = "Debe especificar al menos un área de interés";
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }

                    List<InterestArea> interestAreas = storeDB.InterestAreas.ToList();
                    List<InterestRegion> interestRegions = storeDB.InterestRegions.ToList();
                    JoinTeamViewModel model = GetJoinTeamPage(interestAreas, interestRegions);

                    if (model.TeamInterestArea.Contains("Mostrador") && string.IsNullOrEmpty(model.InterestRegion))
                    {
                        response.Message = "Debe especificar al menos una región de interés";
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }

                    var existe = await storeDB.UserJoinedToTeams.FirstOrDefaultAsync(x => x.Email == model.TeamEmail);
                    if(existe == null)
                    {                    

                        var interestAreaIds = Request.Form["TeamInterestArea"].Split(',');
                        var interestRegionIds = Request.Form["InterestRegion"].Split(',');

                        var user = new UserJoinedToTeam
                        {
                            Names = model.TeamNames,
                            Email = model.TeamEmail,
                            PhoneNumber = model.TeamPhoneNumber,
                            Lastnames = model.TeamLastnames,
                            Address = model.TeamAddress,                            
                            DateCreation = DateTime.Now,
                            InterestArea = model.TeamInterestArea
                        };

                        List<string> areaAddresses = interestAreas.Where(a => interestAreaIds.Contains(a.InterestAreaId.ToString()))
                                                     .SelectMany(a => a.MailCollection.Split(';'))
                                                     .Distinct()
                                                     .ToList();
                        List<string> interestAddresses = null;

                        if (model.TeamInterestArea.Contains("Mostrador"))
                        {
                            interestAddresses = interestRegions.Where(r => interestRegionIds.Contains(r.Id.ToString()))
                                                .SelectMany(r => r.MailCollection.Split(';'))
                                                .Distinct()
                                                .ToList();
                        }

                        bool mailSent = SendMail(areaAddresses, interestAddresses, model);
                        if(mailSent == false)
                        {
                            response.Message = "Ocurrió un error al enviar su solicitud";
                            return Json(response, JsonRequestBehavior.AllowGet);
                        }

                        storeDB.UserJoinedToTeams.Add(user);
                        await storeDB.SaveChangesAsync();
                        response.Success = true;
                        response.Message = "Gracias por unirse a nuestro Equipo";
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                    response.Message = "Ya existe una persona registrada con esa dirección de correo electrónico";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                response.Message = "Lo sentimos, no se pudo realizar la operación. Ha ocurrido un error en el servidor.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                response.Message = "Lo sentimos, no se pudo realizar la operación. Ha ocurrido un error en el servidor.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        private bool SendMail(List<string> areaAddresses, List<string> interestAddresses, JoinTeamViewModel model)
        {

            var configuration = storeDB.MailConfigurations.Where(mc => mc.Active == true).FirstOrDefault();

            if(configuration == null)
            {
                return false;
            }

            string from = configuration.UserName;
            string server = configuration.Server;
            string username = configuration.UserName;
            string password = configuration.Password;

            MailMessage message = new MailMessage();
            areaAddresses.ForEach(a => message.To.Add(a.Trim()));
            if(interestAddresses != null)
            {
                interestAddresses.ForEach(a => {
                    var address = a.Trim();
                    if (message.To.Contains(new MailAddress(address)) == false)
                    {
                        message.To.Add(address);
                    }
                });
            }

            message.From = new MailAddress(from);


            //Poner el asunto del mensaje aquí
            message.Subject = "Registro Unete a mi Equipo";

            //Poner el cuerpo del mensaje aquí
            message.Body = "<strong>Nombre (s):</strong> " + model.TeamNames + "<br /><strong>Apellidos: </strong>" + model.TeamLastnames + "<br /><strong>Tel&eacute;fono: </strong>" + model.TeamPhoneNumber + "<br /><strong>Email: </strong>" + model.TeamEmail + "<br /><strong>Area de Interes: </strong>" + model.TeamInterestArea + "<br /><strong>Direcci&oacute;n: </strong>" + model.TeamAddress;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(server);
            if (configuration.Port > 0)
            {
                client.Port = configuration.Port;
            }

            //Descomentar la línea de abajo si el servidor usa un certificado que no ha sido emitido por una autoridad reconocida
            //ServicePointManager.ServerCertificateValidationCallback = (obj, certificate, chain, sslPolicyErrors) => true;

            client.Credentials = new NetworkCredential(username, password);


            if(configuration.EnableSSL == true)
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

        private JoinTeamViewModel GetJoinTeamPage(List<InterestArea> interestAreas, List<InterestRegion> interestRegions)
        {
            JoinTeamViewModel preview = new JoinTeamViewModel();
            var httpRequest = System.Web.HttpContext.Current.Request;
            foreach (var form in httpRequest.Form.AllKeys)
            {
                var formValue = httpRequest.Form[form];
                formValue = string.IsNullOrEmpty(formValue) ? null : formValue;
                switch (form)
                {
                    case "TeamInterestArea":
                        preview.TeamInterestArea = GetNamesAreas(formValue, interestAreas);
                        break;
                    case "TeamEmail":
                        preview.TeamEmail = formValue;
                        break;
                    case "TeamPhoneNumber":
                        preview.TeamPhoneNumber = formValue;
                        break;
                    case "TeamLastnames":
                        preview.TeamLastnames = formValue;
                        break;
                    case "TeamNames":
                        preview.TeamNames = formValue;
                        break;
                    case "TeamAddress":
                        preview.TeamAddress = formValue;
                        break;
                    case "InterestRegion":
                        if(preview.TeamInterestArea.Contains("Mostrador"))
                        { 
                            preview.InterestRegion = GetRegionNames(formValue,interestRegions);
                        }
                        break;
                }
            }
            return preview;
        }

        private string GetNamesAreas(string formValue, List<InterestArea> listAreas)
        {
            if (string.IsNullOrEmpty(formValue) || formValue == "null")
                return "";
            var f = formValue.Split(',');

            string interetsArea = "";
            for (int i = 0; i < f.Length; i++)
            {
                foreach (var a in listAreas)
                {
                    var id = int.Parse(f[i]);
                    if(id == a.InterestAreaId)
                    {
                        interetsArea += a.InterestAreaName + " ";
                        break;
                    }
                }
            }
            return interetsArea.TrimEnd();
        }

        private string GetRegionNames(string formValue, List<InterestRegion> listAreas)
        {
            if (string.IsNullOrEmpty(formValue) || formValue == "null")
                return "";
            var f = formValue.Split(',');

            string interetsArea = "";
            for (int i = 0; i < f.Length; i++)
            {
                foreach (var a in listAreas)
                {
                    var id = int.Parse(f[i]);
                    if (id == a.Id)
                    {
                        interetsArea += a.Name + " ";
                        break;
                    }
                }
            }
            return interetsArea.TrimEnd();
        }



        private async Task<JoinTeamPage> GetActiveJoinTeamPage()
        {
            var h = await storeDB.JoinTeamPages.FirstOrDefaultAsync(p => p.JoinTeamPageActive);
            if (h != null)
            {
                h.HeadImages = await storeDB.ImageSections.Where(x => x.ImageSectionPageId == h.JoinTeamPageId && x.ImageSectionPageName == "JoinTeamPage").ToListAsync();
                h.InterestAreas = await storeDB.InterestAreas.Where(p => p.InterestAreaActive).ToListAsync();
                h.InterestRegions = await storeDB.InterestRegions.Where(ir => ir.Active == true).ToListAsync();
                return h;
            }
            return null;
        }
    }
}