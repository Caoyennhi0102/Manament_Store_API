using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class SearchDepartmentsByIDController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public SearchDepartmentsByIDController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: SearchDepartmentsByID
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchDepartmentsByID(string MaBP)
        {


            if (string.IsNullOrEmpty(MaBP))
            {
                return Json(new { success = false, message = "Mã bộ phận không được để trống." });
            }
            var department = _sqlConnectionserver.BoPhans.Where(BP => BP.MaBoPhan == MaBP)
                .Select(BP => new
                {
                    BP.MaBoPhan,
                    BP.TenBP,
                    BP.MaCuaHang,
                    TenCuaHang = BP.CuaHang.TenCH
                })
                .FirstOrDefault();
            if (department == null)
            {
                return Json(new { success = false, message = "Không tìm thấy bộ phận với mã đã cho." });
            }
            return Json(new { success = true, data = department });
        }
    }
}