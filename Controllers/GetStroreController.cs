using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class GetStroreController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public GetStroreController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: GetStrore
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetStoreList()
        {
            var stores = _sqlConnectionServer.CuaHangs.ToList();
            return Json(stores, JsonRequestBehavior.AllowGet);
        }
    }
}