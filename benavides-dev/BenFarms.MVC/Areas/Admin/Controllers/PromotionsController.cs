using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BenFarms.MVC.Helper;
using BenFarms.MVC.Models;
using Newtonsoft.Json;

namespace BenFarms.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PromotionsController:Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult UpdatePromotions()
        {
            var service = new BeneficioInteligenteService.PortalBI();
            try
            {
                var promocionesResult = service.ObtenerConsultaPromocion(1, "998877", 1, 1, 0, 1, "", 0, "", "");
                if (promocionesResult.ErrorCode == "CPD1001")
                {
                    var promociones =
                        promocionesResult.PromoPiezasGratisResult.Where(t => t.Segmento.Contains("BI General")).ToList();

                    string pathFile = Server.MapPath("~/Content/promociones.txt");

                    //open file stream
                    if (System.IO.File.Exists(pathFile))
                    {
                        System.IO.File.Delete(pathFile);
                    }
                    using (StreamWriter file = System.IO.File.CreateText(pathFile))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        //serialize object directly into file stream
                        serializer.Serialize(file, promociones);
                    }
                    return
                        Json(
                            new AjaxResponse
                            {
                                Success = true,
                                Message = "Las promociones fueron descargadas correctamente"
                            },
                            JsonRequestBehavior.AllowGet);
                }

                return
                    Json(
                        new AjaxResponse {Success = false, Message = "Ocurrió un error al descargar las promociones."},
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(
                        new AjaxResponse { Success = false, Message = "Ocurrió un error al descargar las promociones." },
                        JsonRequestBehavior.AllowGet);
            }
            

        }
    }
}