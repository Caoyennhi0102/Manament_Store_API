using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class UpdatePositionDefaultController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public UpdatePositionDefaultController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: UpdatePositionDefault
        [HttpGet]
        public ActionResult Index()
        {
            var listCV = _sqlConnectionserver.ChucVus.Include(c => c.BoPhan).ToList();
            ViewBag.DSCV = listCV;
            var bophans = _sqlConnectionserver.BoPhans.ToList();
            ViewBag.DSBoPhan = new SelectList(listCV, "MaBoPhan", "TenBoPhan");
            return View();
        }
       
        [HttpPost]
        public ActionResult UpdatePosition(ChucVu chucVu)
        {
            if (ModelState.IsValid)
            {
                var existingChucVu = _sqlConnectionserver.ChucVus.FirstOrDefault(cv => cv.MaChucVu == chucVu.MaChucVu);
                if (existingChucVu == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy chức vụ cần cập nhật." });
                }
                existingChucVu.TenChucVu = chucVu.TenChucVu;
                existingChucVu.MaBoPhan = chucVu.MaBoPhan;
                _sqlConnectionserver.SaveChanges();

                return Json(new { success = true, message = "Cập nhật chức vụ thành công." });

            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });


        }
    }
}