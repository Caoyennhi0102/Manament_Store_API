using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class GetPermissionsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public GetPermissionsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: GetPermissions
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetPermissions()
        {
            try
            {
                var ListPermissions = _sqlConnectionserver.Quyens.Select(q => new
                {
                    q.MaQuyen,
                    q.TenQuyen
                }).ToList();
                return Json(ListPermissions, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Đã xảy ra lỗi: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}