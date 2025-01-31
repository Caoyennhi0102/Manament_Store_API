using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class GetProductCategoriesController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public GetProductCategoriesController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: GetProductCategories
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProductCategories()
        {
            var categories = _sqlConnectionServer.DanhMucSanPhams.AsEnumerable().Select(c => new
            {
                c.MaDanhMucSP,
                c.TenDanhMuc,
                c.MoTa,
                NgayvaGio = c.NgayvaGio.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();
            return Json(categories, JsonRequestBehavior.AllowGet);
        }
    }
}