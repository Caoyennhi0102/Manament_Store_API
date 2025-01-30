using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class AddPositionController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public AddPositionController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: AddPosition
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
        public ActionResult AddPosition(string TenCV, string maBP)
        {
            try
            {
                if (string.IsNullOrEmpty(TenCV) || string.IsNullOrEmpty(maBP))
                {
                    return Json(new { success = false, message = "Tên chức vụ và bộ phận không được để trống." });
                }

                var IspostionCode = new Service.PositionCode();
                string MaCV = IspostionCode.CreatePositionCode(TenCV);
                var bophans = _sqlConnectionserver.BoPhans.FirstOrDefault(BP => BP.MaBoPhan == maBP);
                if (bophans == null)
                {
                    return Json(new { success = false, message = "Mã bộ phận không tồn tại" });
                }
                var newChucVu = new ChucVu
                {
                    MaChucVu = MaCV,
                    TenChucVu = TenCV,
                    MaBoPhan = maBP,

                };
                _sqlConnectionserver.ChucVus.Add(newChucVu);
                _sqlConnectionserver.SaveChanges();
                return Json(new
                {
                    success = true,
                    MaChucVu = MaCV,
                    TenChucVu = TenCV,
                    TenBoPhan = bophans.TenBP

                });


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình thêm chức vụ.", error = ex.Message });
            }
        }
    }
}