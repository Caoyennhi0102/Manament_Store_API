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
            return View();
        }

        public ActionResult Approve(int userID, string Action, string OperationType)
        {
            try
            {
                var user = _sqlConnectionserver.Users.FirstOrDefault(u => u.UserID == userID);
                if (user == null)
                {
                    return Json(new { success = false, message = "User không tồn tại." });
                }
                if (OperationType == "Add")
                {
                    if (Action == "Approve")
                    {
                        user.TrangThaiDuyetQL = "Approve";
                        user.DNLanDau = true;
                        _sqlConnectionserver.SaveChanges();

                        var emailService = new Service.EmailService();
                        string subject = "User của bạn đã được phê duyệt";
                        string body = $"Xin chào {user.UserName}{user.PasswordHash},<br/>User của bạn đã được phê duyệt. Vui lòng đăng nhập để đổi mật khẩu để sử dụng.";
                        emailService.SendEmail(user.NhanVien.Email, subject, body);

                        return Json(new { success = true, message = "Tài khoản đã được phê duyệt." });
                    }
                    else if (Action == "Reject")
                    {
                        user.TrangThaiDuyetQL = "Reject";
                        _sqlConnectionserver.SaveChanges();

                        return Json(new { success = true, message = "Tài khoản đã bị từ chối." });

                    }
                }
                // Logic xử lý cho cập nhật user
                if (OperationType == "Update")
                {
                    if (Action == "Approve")
                    {
                        user.TrangThaiDuyetQL = "Approve";
                        _sqlConnectionserver.SaveChanges();

                        var emailService = new Service.EmailService();
                        string subject = "User của bạn đã được cập nhật và phê duyệt";
                        string body = $"Xin chào {user.UserName},<br/>Thông tin tài khoản của bạn đã được cập nhật và phê duyệt. Vui lòng đăng nhập để sử dụng.";
                        emailService.SendEmail(user.NhanVien.Email, subject, body);

                        return Json(new { success = true, message = "Cập nhật tài khoản đã được phê duyệt." });
                    }
                    else if (Action == "Reject")
                    {
                        user.TrangThaiDuyetQL = "Reject";
                        _sqlConnectionserver.SaveChanges();

                        return Json(new { success = true, message = "Cập nhật tài khoản đã bị từ chối." });
                    }
                }
                // Logic xử lý cho xóa user
                if (OperationType == "Delete")
                {
                    if (Action == "Approve")
                    {
                        _sqlConnectionserver.Users.Remove(user);
                        _sqlConnectionserver.SaveChanges();

                        var emailService = new Service.EmailService();
                        string subject = "User của bạn đã bị xóa";
                        string body = $"Xin chào {user.UserName},<br/>Tài khoản của bạn đã bị xóa khỏi hệ thống. Nếu có vấn đề gì, vui lòng liên hệ với bộ phận quản lý.";
                        emailService.SendEmail(user.NhanVien.Email, subject, body);

                        return Json(new { success = true, message = "Tài khoản đã bị xóa." });
                    }
                    else if (Action == "Reject")
                    {
                        return Json(new { success = true, message = "Xóa tài khoản đã bị từ chối." });
                    }
                }
                return Json(new { success = false, message = "Hành động không hợp lệ." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

    }
}