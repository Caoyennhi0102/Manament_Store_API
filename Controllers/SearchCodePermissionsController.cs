using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class SearchCodePermissionsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public SearchCodePermissionsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: SearchCodePermissions
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchCodePermissions(string maQuyen)
        {
            try
            {
                if (string.IsNullOrEmpty(maQuyen))
                {
                    return Json(new { success = false, message = "Mã quyền không được để trống." });
                }

                if (_sqlConnectionserver == null)
                {
                    return Json(new { success = false, message = "Kết nối cơ sở dữ liệu không khả dụng." });
                }

                var quyen = _sqlConnectionserver.Quyens.FirstOrDefault(u => u.MaQuyen == maQuyen);

                if (quyen != null)
                {
                    return Json(new { success = true, data = quyen });
                }

                return Json(new { success = false, message = "Không tìm thấy quyền!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }
    }
}