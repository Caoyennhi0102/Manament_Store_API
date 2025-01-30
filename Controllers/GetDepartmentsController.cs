using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class GetDepartmentsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public GetDepartmentsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: GetDepartments
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetDepartments()
        {
            try
            {
                var departments = _sqlConnectionserver.BoPhans.Select(
                    bp => new
                    {
                        bp.MaBoPhan,
                        bp.TenBP,
                        TenCuaHang = bp.CuaHang.TenCH
                    })
                    .ToList();
                return Json(departments, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra khi tải danh sách bộ phận: {ex.Message}" });
            }
        }
    }
}