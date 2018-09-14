using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BenavidesFarm.DataModels.Models;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Data.Entity;

namespace BenFarms.MVC.Controllers
{
    [RoutePrefix("v1/branches")]
    public class BranchesController : ApiController
    {
        readonly MyApplicationDbContext storeDB;

        public BranchesController()
        {
            storeDB = new MyApplicationDbContext();
        }

        [HttpGet]
        public HttpResponseMessage Fose()
        {
            var branchs = storeDB.Branchs.Where(b => b.BranchActive == true && b.BranchFose == true).Include(b => b.City).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, branchs);
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var branchs = GetActiveBranchs();
            return Request.CreateResponse(HttpStatusCode.OK, branchs);
        }

        [HttpGet]
        public HttpResponseMessage GetNo7()
        {
            var branchs = GetActiveBranchs24();
            return Request.CreateResponse(HttpStatusCode.OK, branchs);
        }


        private List<Branch> GetActiveBranchs()
        {
            return storeDB.Branchs.Where(x => x.BranchActive).Include(b => b.State).Include(b => b.City).ToList();
        }



        private List<Branch> GetActiveBranchs24()
        {

            //            string[] Sucursales = new string[] {
            //"2L0131","2L0132","2L0139","2L0141","2L0143","2L0152","2L0155","2L0162","2L0195","2L0197","2L0207","2L0219","2L0227","2L0236","2L0251","2L0253","2L0262","2L0265","2L0315","2L0316","2L0317","2L0326",
            //"2L0614","2L0634","2L0654","2L0655","2L0698","2L0770","2L0778","2L0809","2L0895","2L1056","2L1058","2L1087","2L1127","2L1137","2L1146","2L1157","2L1199","2L1204","2L1228","2L1229","2L1260","2L1279","2L1301","2L1325"
            //            };






            string[] Sucursales = new string[] { "2L0131", "2L0132", "2L0139", "2L0141", "2L0143", "2L0152", "2L0195", "2L0227", "2L0236", "2L0262", "2L0265", "2L0614", "2L1058", "2L1127" };


            List<Branch> BranchS =  storeDB.Branchs.Where(x => Sucursales.Contains(x.BranchCeco)).Include(b => b.State).Include(b => b.City).ToList();
            return BranchS;
        }

    }
}