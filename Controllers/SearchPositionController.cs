using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class SearchPositionController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public SearchPositionController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: SearchPosition
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchPosition(string maCV)
        {
            try
            {
                if (string.IsNullOrEmpty(maCV))
                {
                    return Json(new { success = false, message = "Mã chức vụ không được để trống." });
                }

                var chucvu = _sqlConnectionserver.ChucVus.FirstOrDefault(cv => cv.MaChucVu == maCV);
                if (chucvu == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy chức vụ." });
                }
                return Json(new
                {
                    success = true,
                    MaChucVu = chucvu.MaChucVu,
                    TenCV = chucvu.TenChucVu,
                    MaBP = chucvu.MaBoPhan,
                    tenBoPhan = chucvu.BoPhan?.TenBP


                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình tìm kiếm mã chức vụ{ex.Message}" });
            }
        }
    }
}