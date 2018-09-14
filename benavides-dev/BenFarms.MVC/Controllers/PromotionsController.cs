using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BenFarms.MVC.BeneficioInteligenteService;
using BenFarms.MVC.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Helpers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;


using BenFarms.MVC.Services;
using BenavidesFarm.DataModels.Models.Pages;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Data.Entity;


using System.Net.Mail;


namespace BenFarms.MVC.Controllers
{
    public class PromotionsController : Controller
    {
        readonly MyApplicationDbContext storeDB;
        private BeneficioInteligenteService.PortalBI service = new PortalBI();


        public PromotionsController()
        {
            storeDB = new MyApplicationDbContext();
        }


        // GET: Pillar
        [AllowAnonymous]
        public  ActionResult Index()
        {

            return View();
        }


        [AllowAnonymous]
        public ActionResult BirthdayView()
        {

            var card = Request.QueryString["Card"];
            var service = new BeneficioInteligenteService.PortalBI();
            var   Result  = service.PromoCumpleanios(1, "998877", "1", card);
            return View((new BirthdayViewModel(card, Result.ErrorCode, Result.Error)));
        }


        [AllowAnonymous]
        public ActionResult SADCuponesView()
        {
            return View("SADCuponesView");
        }


        [AllowAnonymous]
        public ActionResult DinamicaHuggies()
        {
            return View("DinamicaHuggies");
        }
        //DinamicaHuggies

        //  public async Task<ActionResult> Index()
        [AllowAnonymous]
        public async Task<ActionResult> landingno7Serum()
        {
            var branchPage = await GetActiveBranchPage();
            //  var estados = await GetStates();

            // var Ciudades = (IEnumerable<SelectListItem>)ViewBag.Ciudades;
            int[] Cities = new int[] { 115, 116, 118, 119 };
         //   int[] Cities = new int[] { 38, 115, 116, 118, 119, 123, 124, 130, 134 };
                //em.Estado.Id == id'
            var Ciud =      await storeDB.EstadosMunicipios.Where(x => Cities.Contains(x.Municipio.Id)).Select(x => x.Municipio).OrderBy(m => m.Name).ToListAsync();



            List<SelectListItem> CiudadesList = new List<SelectListItem>();

            //IEnumerable <SelectListItem> CiudadesList;
              CiudadesList = Ciud.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name }).ToList();


            CiudadesList.Insert(0, new SelectListItem { Value = "0", Text = "Seleccione Ciudad", Selected = true });

          //  CiudadesList.Add(new SelectListItem { Value = "0", Text = "Seleccione Ciudad", Selected=true });
            ViewBag.Ciudades = CiudadesList;

            if (branchPage != null)
                return View(branchPage);

            return RedirectToAction("NotFound", "Error");


            //return View("landingno7");
        }


        public async Task<ActionResult> landingno7()
        {
            var branchPage = await GetActiveBranchPage();
            //  var estados = await GetStates();

            // var Ciudades = (IEnumerable<SelectListItem>)ViewBag.Ciudades;
            int[] Cities = new int[] { 115, 116, 118, 119 };
            //   int[] Cities = new int[] { 38, 115, 116, 118, 119, 123, 124, 130, 134 };
            //em.Estado.Id == id'
            var Ciud = await storeDB.EstadosMunicipios.Where(x => Cities.Contains(x.Municipio.Id)).Select(x => x.Municipio).OrderBy(m => m.Name).ToListAsync();



            List<SelectListItem> CiudadesList = new List<SelectListItem>();

            //IEnumerable <SelectListItem> CiudadesList;
            CiudadesList = Ciud.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name }).ToList();


            CiudadesList.Insert(0, new SelectListItem { Value = "0", Text = "Seleccione Ciudad", Selected = true });

            //  CiudadesList.Add(new SelectListItem { Value = "0", Text = "Seleccione Ciudad", Selected=true });
            ViewBag.Ciudades = CiudadesList;

            if (branchPage != null)
                return View(branchPage);

            return RedirectToAction("NotFound", "Error");


            //return View("landingno7");
        }




        private async Task<BranchPage> GetActiveBranchPage()
        {
            var h = await storeDB.BranchPages.FirstOrDefaultAsync();
            if (h != null)
            {
                h.HeadImages = await storeDB.ImageSections.Where(x => x.ImageSectionPageId == h.BranchPageId && x.ImageSectionPageName == "BranchPage").ToListAsync();
               // h.Branchs = await GetActiveBranchs24();
                return h;
            }
            return null;
        }

        private async Task<List<Branch>> GetActiveBranchs24()
        {

            //            string[] Sucursales = new string[] {
            //"2L0131","2L0132","2L0139","2L0141","2L0143","2L0152","2L0155","2L0162","2L0195","2L0197","2L0207","2L0219","2L0227","2L0236","2L0251","2L0253","2L0262","2L0265","2L0315","2L0316","2L0317","2L0326",
            //"2L0614","2L0634","2L0654","2L0655","2L0698","2L0770","2L0778","2L0809","2L0895","2L1056","2L1058","2L1087","2L1127","2L1137","2L1146","2L1157","2L1199","2L1204","2L1228","2L1229","2L1260","2L1279","2L1301","2L1325"
            //            };


            string[] Sucursales = new string[] { "2L0131", "2L0132", "2L0139", "2L0141", "2L0143", "2L0152", "2L0195", "2L0227", "2L0236", "2L0262", "2L0265", "2L0614", "2L1058", "2L1127" };

     


            List<Branch> BranchS =  await  storeDB.Branchs.Where(x => Sucursales.Contains(x.BranchCeco)).ToListAsync();
            return BranchS;
        }


        private async Task<List<Estados>> GetStates()
        {
            return await storeDB.Estados.OrderBy(st => st.Name).ToListAsync();
        }

        private async Task<List<Municipios>> GetCitiesFromState(int? id)
        {
            if (id != null)
            {

                                int[] Cities = new int[] { 115, 116, 118, 119 };
              //  int[] Cities = new int[] { 38, 115, 116, 118, 119,123, 124, 130, 134 };
                   //em.Estado.Id == id'
                return await storeDB.EstadosMunicipios.Where(x => Cities.Contains(x.Municipio.Id)).Select(x => x.Municipio).OrderBy(m => m.Name).ToListAsync();
            }
            return new List<Municipios>();
        }


//        private async Task<List<Branch>> GetBranches(int? id)
//        {
//            if (id != null)
//            {
//                // int[] Cities = new int[] { 38, 115, 116, 118, 119, 123, 124, 130, 134 };
//                //em.Estado.Id == id'
//                //    return await storeDB.EstadosMunicipios.Where(x => Cities.Contains(x.Municipio.Id)).Select(x => x.Municipio).OrderBy(m => m.Name).ToListAsync();


//                string[] BrancCecoList = new string[] {"2L0131","2L0132","2L0139","2L0141","2L0143","2L0152","2L0155","2L0162","2L0195","2L0197","2L0207","2L0219","2L0227","2L0236","2L0251","2L0253","2L0262",
//"2L0265","2L0315","2L0316","2L0317","2L0326","2L0614","2L0634","2L0654","2L0655","2L0698","2L0770","2L0778","2L0809","2L0895","2L1056","2L1058","2L1087","2L1127",
//"2L1137","2L1146","2L1157","2L1199","2L1204","2L1228","2L1229","2L1260","2L1279","2L1301","2L1325" };

//                List<SelectListItem> BranchesList = new List<SelectListItem>();
//                //IEnumerable <SelectListItem> CiudadesList;
//            //    CiudadesList = Ciud.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Name }).ToList();


//                return await storeDB.Branchs.Where(x => BrancCecoList.Contains(x.BranchCeco)).Select(xk
                    
//                    //7.OrderBy(m => m.Name).ToListAsync();


//            }
//            return new List<Municipios>();
//        }


        [HttpGet]
        public async Task<JsonResult> GetBranches(int? id)
        {

            if (id == 0)
            {
                //    string[] BrancCecoList = new string[] {"2L0131","2L0132","2L0139","2L0141","2L0143","2L0152","2L0155","2L0162","2L0195","2L0197","2L0207","2L0219","2L0227","2L0236","2L0251","2L0253","2L0262",
                //"2L0265","2L0315","2L0316","2L0317","2L0326","2L0614","2L0634","2L0654","2L0655","2L0698","2L0770","2L0778","2L0809","2L0895","2L1056","2L1058","2L1087","2L1127",
                //"2L1137","2L1146","2L1157","2L1199","2L1204","2L1228","2L1229","2L1260","2L1279","2L1301","2L1325" };

                string[] BrancCecoList = new string[] { "2L0131", "2L0132", "2L0139", "2L0141", "2L0143", "2L0152", "2L0195", "2L0227", "2L0236", "2L0262", "2L0265", "2L0614", "2L1058", "2L1127" };


                var Sucursales = storeDB.Branchs.Where(x => BrancCecoList.Contains(x.BranchCeco)).Select(x => x).ToList();


               // var cities = await GetCitiesFromState(id);
                return Json(Sucursales.Select(e => new { Value = e.BranchId, Text = e.BranchName }), JsonRequestBehavior.AllowGet);
            }
            else
            {
                //    string[] BrancCecoList = new string[] {"2L0131","2L0132","2L0139","2L0141","2L0143","2L0152","2L0155","2L0162","2L0195","2L0197","2L0207","2L0219","2L0227","2L0236","2L0251","2L0253","2L0262",
                //"2L0265","2L0315","2L0316","2L0317","2L0326","2L0614","2L0634","2L0654","2L0655","2L0698","2L0770","2L0778","2L0809","2L0895","2L1056","2L1058","2L1087","2L1127",
                //"2L1137","2L1146","2L1157","2L1199","2L1204","2L1228","2L1229","2L1260","2L1279","2L1301","2L1325" };

                string[] BrancCecoList = new string[] { "2L0131", "2L0132", "2L0139", "2L0141", "2L0143", "2L0152", "2L0195", "2L0227", "2L0236", "2L0262", "2L0265", "2L0614", "2L1058", "2L1127" };

                var Sucursales = storeDB.Branchs.Where(x => BrancCecoList.Contains(x.BranchCeco) && (x.City.Id == id)).Select(x => x).ToList();


                //var cities = await GetCitiesFromState(id);
                return Json(Sucursales.Select(e => new { Value = e.BranchId, Text = e.BranchName }), JsonRequestBehavior.AllowGet);

            }
        }

        //private async Task<List<Municipios>> GetCitiesFromState(int? id)
        //{
        //    if (id != null)
        //    {
        //        int[] Cities = new int[] { 38, 115, 116, 118, 119, 123, 124, 130, 134 };
        //        //em.Estado.Id == id'
        //        return await storeDB.EstadosMunicipios.Where(x => Cities.Contains(x.Municipio.Id)).Select(x => x.Municipio).OrderBy(m => m.Name).ToListAsync();
        //    }
        //    return new List<Municipios>();
        //}





        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var response = new AjaxResponse { Success = false };
            var branchs = await GetActiveBranchs();
            //return Request.CreateResponse(HttpStatusCode.OK, branchs);
            response.Message = "Success";
            response.Data = branchs;
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public async Task<JsonResult> GetCities(int? id)
        {
            var cities = await GetCitiesFromState(id);
            return Json(cities.Select(e => new { Value = e.Id.ToString(), Text = e.Name }), JsonRequestBehavior.AllowGet);
        }
        private async Task<List<Branch>> GetActiveBranchs()
        {
            return await storeDB.Branchs.Where(x => x.BranchActive).ToListAsync();
        }


        [AllowAnonymous]
        public async Task<ActionResult> SearchLandingNo7(BranchSearchInputModel searchCriteria)
        
{
            var branchPage = await GetActiveBranchPage();

            if (ModelState.IsValid == false)
            {
                int a = 0;
            }

            if (branchPage != null)
            {
                BranchesApplicationService service = new BranchesApplicationService(storeDB);
                BranchSearchResultViewModel result = service.getBranchesByLocaltionriteria2(searchCriteria);
                ViewBag.SearchResults = result;
                return View(branchPage);
            }

            return RedirectToAction("NotFound", "Error");

        }


        [AllowAnonymous]
        public async Task<ActionResult> SearchLandingNo7Serum(BranchSearchInputModel searchCriteria)

        {
            var branchPage = await GetActiveBranchPage();

            if (ModelState.IsValid == false)
            {
                int a = 0;
            }

            if (branchPage != null)
            {
                BranchesApplicationService service = new BranchesApplicationService(storeDB);
                BranchSearchResultViewModel result = service.getBranchesByLocaltionriteria2(searchCriteria);
                ViewBag.SearchResults = result;
                return View(branchPage);
            }

            return RedirectToAction("NotFound", "Error");

        }


      

        [AllowAnonymous]
        public ActionResult Descarga()
        {
            ViewBag.Page = "Billing";
            //var billingPage = await GetActiveBillingPage();
            //if (billingPage != null)
            return View("Descarga");
            // return RedirectToAction("NotFound", "Error");
        }


        public ActionResult BuzonSugerencias()
        {
            //ViewBag.Page = "Billing";
            //var billingPage = await GetActiveBillingPage();
            //if (billingPage != null)
            return View();
            // return RedirectToAction("NotFound", "Error");
        }


        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Contacto()
        {
            var response = new AjaxResponse { Success = false };
            try
            {
                if (ModelState.IsValid)
                {
                    var httpRequest = System.Web.HttpContext.Current.Request;
                    var nombre = string.Empty;
                    var comments = string.Empty;
                    var email = string.Empty;
                    var sucursal = string.Empty;

                    foreach (var form in httpRequest.Form.AllKeys)
                    {
                        var formValue = httpRequest.Form[form];
                        formValue = string.IsNullOrEmpty(formValue) ? null : formValue;

                        switch (form)
                        {
                            case "Nombre":
                                nombre = formValue;
                                break;
                            case "Comments":
                                comments = formValue;
                                break;
                            case "Email":
                                email = formValue;
                                break;
                            case "Sucursal":
                                sucursal = formValue;
                                break;
                                                        }
                    }

                    bool mailSent = SendMail(email, nombre, comments, sucursal );
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
                response.Message = "Ha ocurrido un error interno en el servidor";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        private bool SendMail(string email, string nombre, string comments, string sucursal)
        {

            var configuration = storeDB.MailConfigurations.Where(mc => mc.Active == true).FirstOrDefault();

            if (configuration == null)
            {
                return false;
            }

            string from = "webmaster@benavides.com.mx";
            string server = "10.1.0.132";
            string username = "webmaster@benavides.com.mx";
            string password = "benavides.3";
            //const string to = "contactobi@benavides.com.mx";
            const string to = "jmenar@benavides.com.mx";

            //jmenar@benavides.com.mx

            MailMessage message = new MailMessage();

            message.To.Add(to);
            message.From = new MailAddress(from);

            //Poner el asunto del mensaje aquí
            //message.Subject = subject;
            message.Subject = "Nueva Sugerencia  de:" + ((string.IsNullOrEmpty(nombre)) ? "ANONIMO" : nombre) + " sucursal:" + ((string.IsNullOrEmpty(sucursal)) ? "ANONIMO" : sucursal);
            //Poner el cuerpo del mensaje aquí
            // Nueva Sugerencia  de:    de la sucursal : 
            message.Body = "<strong>Nombre:</strong> " + ((string.IsNullOrEmpty(nombre)) ? "ANONIMO" : nombre) + "<br /><strong>Sucursal:</strong> " + ((string.IsNullOrEmpty(sucursal)) ? "ANONIMO" : sucursal) + "<br /><strong>Correo Electrónico:</strong> " +  email  + "<br /><br /><strong>Comentarios: </strong>" + comments ;
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




        [AllowAnonymous]
        public async System.Threading.Tasks.Task<ActionResult> EnviaEmail()
        {


            var Gettoken = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //MMH forzar uso de protocolo de seguridad diferente
            Gettoken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Gettoken.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string requestUriToken = "https://auth.exacttargetapis.com/v1/requestToken";
            JObject JsonToken = JObject.Parse(@"
                {'clientId': 'j3h962ggpsbbb2k4i22nlcvu',
    'clientSecret': 'PilLStqb09Pjt7XmskQ2g86V'
               } ");


            HttpResponseMessage responseToken = await Gettoken.PostAsJsonAsync(requestUriToken, JsonToken);
            var result = await responseToken.Content.ReadAsStringAsync();


            //var responseToken = await Gettoken.PostAsJsonAsync(requestUriToken, JsonToken);
            //var res = responseToken.Content;

            //res.ReadAsStringAsync(); 

            using (var client = new HttpClient())
            {
                string requestUri = "https://www.exacttargetapis.com/messaging/v1/messageDefinitionSends/key:2092/send";
                string token = "7uJLaNyxEkdtH0wks3mtbPgr";
                JObject Json = JObject.Parse(
                    @"

{
'To':
{
'Address':'rortega@benavides.com.mx',
'SubscriberKey':'15025429524',
'ContactAttributes':
{
'SubscriberAttributes':
{
'SubscriberKey': '15025429524',
'EmailAddress': 'rortega@benavides.com.mx',
'ConfirmSubscriptionLink': 'https://axa.mx/web/conoce-axa',
'DateForSend':'2018-08-20 16:30'
}
}
}
}

");

                //ortegac2990@gmail.com
                //15088406101


                //  client.PostAsJsonAsync<>(requestUri, token);
                //Json.Add("To", Json.Add("Addres", ""));                



                //HttpResponseMessage response = await client.PostAsJsonAsync(requestUri, json);
                //var result = await response.Content.ReadAsStringAsync();


                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //MMH forzar uso de protocolo de seguridad diferente
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.PostAsJsonAsync(requestUri, Json);
                // HttpResponseMessage response =  client.PostAsJsonAsync(requestUri, Json);
                var resultado2 = await   response.Content.ReadAsStringAsync();
                return null;
                //return null;
            }




        }

        [HttpGet]
        public JsonResult LoadTickets(string card)
        {
            var resultTickets = service.ConsultarTickets("1", "998877", "1", card, 100000);
            var returnValueTickets = resultTickets.ReturnValue;
            List<TicketViewModel> tickets = new List<TicketViewModel>();

            if (returnValueTickets != null)
                foreach (var ticket in returnValueTickets)
                {
                    decimal dineroElectronico = 0;
                    decimal importe = 0;
                    foreach (var articulo in ticket.Articulos)
                    {
                        //dineroElectronico += articulo.DineroElectronico;
                        //importe += articulo.Importe;
                        tickets.Add(new TicketViewModel
                        {
                            DineroElectronico = articulo.DineroElectronico,
                            Fecha = ticket.Fecha + " " + ticket.Hora,
                            Sucursal = ticket.Sucursal,
                            Ticket = ticket.NumTicket,
                            TipoMovimiento = ticket.TipoMovimiento,
                            Importe = articulo.Importe,
                            Producto = articulo.Nombre
                        });
                    }
                    //tickets.Add(new TicketViewModel
                    //{
                    //    DineroElectronico = dineroElectronico, Fecha = ticket.Fecha + " " + ticket.Hora, Sucursal = ticket.Sucursal, Ticket = ticket.NumTicket, TipoMovimiento = ticket.TipoMovimiento, Importe = ticket.Importe

                    //});
                
                }

            var jsonData = new
            {
                data = from ticket in tickets select ticket
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult LoadPromocionesDiarias(string card)
        {
            var service = new BeneficioInteligenteService.PortalBI();
            try
            {
                var promocionesResult = service.ObtenerConsultaPromocion(1, "998877", 1, 1, 0, 1, "", 0, "", "");
                if (promocionesResult.ErrorCode == "CPD1001")
                {
                    var promociones =
                        promocionesResult.PromoPiezasGratisResult.Where(t => t.Segmento.Contains("BI General")).ToList();


                    var jsonData = new
                    {
                        data = promocionesResult.PromoPiezasGratisResult.Where(t => t.Segmento.Contains("BI General")).ToList()
                    };
                    return Json(jsonData, JsonRequestBehavior.AllowGet);


                    //return
                    //    Json(
                    //        new AjaxResponse
                    //        {
                    //            Success = true,
                    //            Message = "Las promociones fueron descargadas correctamente"
                    //        },
                    //        JsonRequestBehavior.AllowGet);
                }

                //    return
                //        Json(
                //            new AjaxResponse { Success = false, Message = "Ocurrió un error al descargar las promociones." },
                //            JsonRequestBehavior.AllowGet);
                //}
                //catch (Exception)
                //{
                //    return Json(
                //            new AjaxResponse { Success = false, Message = "Ocurrió un error al descargar las promociones." },
                //            JsonRequestBehavior.AllowGet);
                //}


                return Json(
                        new AjaxResponse { Success = false, Message = "Ocurrió un error al descargar las promociones." },
                        JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(
                        new AjaxResponse { Success = false, Message = "Ocurrió un error al descargar las promociones." },
                        JsonRequestBehavior.AllowGet);
            }
        }




        [HttpGet]
        public JsonResult LoadAcumulacion(string card)
        {
            //var service = new BeneficioInteligenteService.PortalBI();
            var resultAcumulacionPendiente = service.ConsultaAcumulacionPendiente("1", "998877", "1", "2311", card);
            List<PiezaGratisViewModel> piezasGratis = new List<PiezaGratisViewModel>();

            if (resultAcumulacionPendiente.CodigoRespuesta_StrCodigo!= "0")
            {
                var returnPiezasGratis = resultAcumulacionPendiente.Data;

                foreach (var pieza in returnPiezasGratis)
                {

                    piezasGratis.Add(new PiezaGratisViewModel
                    {
                        Acumuladas = pieza.Acumulado,
                        Producto = pieza.NombreProducto,
                        Regla = pieza.DescripcionMecanica + " obtén " + pieza.Cupon + pieza.NombreProducto,
                        Vigencia = pieza.VigenciaFin
                    });

                }
            }

            var jsonData = new
            {
                data = from pieza in piezasGratis select pieza
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult LoadPromocionesDisponibles(string card)
        {
            //var service = new BeneficioInteligenteService.PortalBI();
            var resultPromocionesDisponibles = service.ConsultarPromocionDisponible(1, "998877", "1", card);
            List<PromocionViewModel> promociones = new List<PromocionViewModel>();

            if (resultPromocionesDisponibles.ErrorCode == "CPD1001")
            {
                var returnPromociones = resultPromocionesDisponibles.ReturnValue;

                foreach (var promocion in returnPromociones)
                {
                    if (!string.IsNullOrEmpty(promocion.Mensaje) && promocion.Mensaje != " ")
                    {
                        promociones.Add(new PromocionViewModel
                        {

                            Cupon = promocion.Cupon,
                            Promocion = promocion.Mensaje,
                            Vigencia = promocion.Vigencia
                        });
                    }
                }
            }

            var jsonData = new
            {
                data = from promocion in promociones select promocion
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}