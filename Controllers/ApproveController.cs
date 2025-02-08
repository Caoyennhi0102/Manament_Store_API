using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class ApproveController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public ApproveController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: Approve
        public ActionResult Index()
        {
            var users = _sqlConnectionserver.Users.ToList();
            return View(users);
        }
        [HttpPost]
        public JsonResult ApproveUser(int userId)
        {
            try
            {
                var user = _sqlConnectionserver.Users.FirstOrDefault(u => u.UserID == userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy User." });
                }

                // Cập nhật trạng thái phê duyệt
                user.TrangThai = "Đã Duyệt";
                user.TrangThaiDuyetQL = "Đã Duyệt";
                _sqlConnectionserver.SaveChanges();

                return Json(new { success = true, message = "User đã được phê duyệt." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }
        [HttpPost]
        public JsonResult RejectUser(int userId)
        {
            try
            {
                var user = _sqlConnectionserver.Users.FirstOrDefault(u => u.UserID == userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy User." });
                }

                // Cập nhật trạng thái từ chối
                user.TrangThai = "Từ Chối";
                user.TrangThaiDuyetQL = "Từ Chối";
                _sqlConnectionserver.SaveChanges();

                return Json(new { success = true, message = "User đã bị từ chối." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

    }
}