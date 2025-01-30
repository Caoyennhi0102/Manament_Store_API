using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
   // [Authorize(Roles ="Admin")]
    public class UpdateDepartmentsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;
        public UpdateDepartmentsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: UpdateDepartments
        [HttpGet]
        public ActionResult Index()
        {
            var stores = _sqlConnectionserver.CuaHangs
            .Select(ch => new { ch.MaCuaHang, ch.TenCH })
            .ToList();
            ViewBag.Store = new SelectList(stores, "MaCuaHang", "TenCH");
            return View();
        }
        
        
        [HttpPost]
        public ActionResult UpdateDepartments(string maBoPhan, string tenBoPhan, int? maCuaHang)
        {
            try
            {
                if (string.IsNullOrEmpty(maBoPhan) || string.IsNullOrEmpty(tenBoPhan) || maCuaHang == null)
                {
                    return Json(new { success = false, message = "Thông tin bộ phận không được để trống." });
                }
                var department = _sqlConnectionserver.BoPhans.FirstOrDefault(bp => bp.MaBoPhan == maBoPhan);

                if (department == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy bộ phận để cập nhật." });
                }
                department.TenBP = tenBoPhan;
                department.MaCuaHang = (int)maCuaHang;
                _sqlConnectionserver.SaveChanges();
                return Json(new { success = true, message = "Cập nhật bộ phận thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }
    }
}