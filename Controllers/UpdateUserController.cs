using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UpdateUserController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public UpdateUserController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: UpdateUser
        [HttpGet]
        public ActionResult Index()
        {
            var users = _sqlConnectionserver.Users.ToList();
            return View(users);
        }
        [HttpPost]
        public ActionResult UpdateUser(User userUpdate, int? MaNV)
        {
            try
            {
                if (MaNV == null || MaNV <= 0)
                {
                    return Json(new { success = false, message = "Vui lòng nhập mã nhân viên hợp lệ" });
                }
                var user = _sqlConnectionserver.Users.FirstOrDefault(u => u.MaNhanVien == MaNV);
                if (user == null)
                {
                    return Json(new { success = false, message = "Mã nhân viên không tồn tại." });
                }

                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy User để cập nhật." });
                }
                var nhanvien = _sqlConnectionserver.NhanViens.FirstOrDefault(u => u.MaNhanVien == MaNV);
                user.MaNhanVien = userUpdate.MaNhanVien;
                user.Roles = userUpdate.Roles;
                user.TrangThai = userUpdate.TrangThai = "Chờ Duyệt";
                user.TrangThaiDuyetQL = userUpdate.TrangThaiDuyetQL = "Chờ Duyệt";
                _sqlConnectionserver.SaveChanges();

                var managerEmail = _sqlConnectionserver.NhanViens
          .Where(nv =>
              nv.MaChucVu == _sqlConnectionserver.ChucVus
                  .Where(cv => cv.TenChucVu == "Quản lý cửa hàng")
                  .Select(cv => cv.MaChucVu)
                  .FirstOrDefault() &&
              nv.MaBoPhan == nhanvien.MaBoPhan)
          .Select(nv => nv.Email)
          .FirstOrDefault();
                if (string.IsNullOrEmpty(managerEmail))
                {
                    return Json(new { success = true, message = "User đã được tạo nhưng không tìm thấy Quản lý cửa hàng để gửi email phê duyệt." });
                }
                var emailService = new Service.EmailService();
                string subject = "Phê duyệt User mới";
                string body = $@"
        Xin chào Quản lý,<br/><br/>
        Nhân viên {user.MaNhanVien} (UserID: {user.UserID}) đã được cập nhật thông tin.<br/>
        Vui lòng phê duyệt hoặc từ chối tài khoản tại đường dẫn sau:<br/>
        <a href='https://yourapp.com/User/Approve?UserId={user.UserID}&Action=Approve'>Phê duyệt</a> | 
        <a href='https://yourapp.com/User/Approve?UserId={user.UserID}&Action=Reject'>Từ chối</a>";

                emailService.SendEmail(managerEmail, subject, body);
                return Json(new { success = true, message = "User đã được cập nhật và email phê duyệt đã được gửi đến quản lý." });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = $"User đã được cập nhật nhưng không thể gửi email phê duyệt. Chi tiết lỗi: {ex.Message}" });
            }




        }

    }
}