using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DeleteUserController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public DeleteUserController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: DeleteUser
        [HttpGet]
        public ActionResult Index()
        {
            var users = _sqlConnectionserver.Users.ToList();

            return View(users);
        }
        [HttpPost]
        public ActionResult DeleteUser(int userId, int? MaNV)
        {
            try
            {
                if (MaNV == null || MaNV < 0)
                {
                    return Json(new { success = false, message = "Không được để trống hoặc mã nhân phải lớn hơn 0" });

                }
                var userDelete = _sqlConnectionserver.Users.FirstOrDefault(u => u.MaNhanVien == MaNV);
                if (userDelete == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy User tương ứng với mã nhân viên cung cấp " });

                }


                var nhanvien = _sqlConnectionserver.NhanViens.FirstOrDefault(u => u.MaNhanVien == MaNV);
                userDelete.TrangThai = "Chờ Duyệt";
                userDelete.TrangThaiDuyetQL = "Chờ duyệt";
                _sqlConnectionserver.Users.Remove(userDelete);
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
        Nhân viên {userDelete.MaNhanVien} (UserID: {userDelete.UserID}) đã được xóa .<br/>
        Vui lòng phê duyệt hoặc từ chối tài khoản tại đường dẫn sau:<br/>
        <a href='https://yourapp.com/User/Approve?UserId={userDelete.UserID}&Action=Approve'>Phê duyệt</a> | 
        <a href='https://yourapp.com/User/Approve?UserId={userDelete.UserID}&Action=Reject'>Từ chối</a>";

                emailService.SendEmail(managerEmail, subject, body);
                return Json(new { success = true, message = "User đã được xóa khỏi hệ thống và email phê duyệt đã được gửi đến quản lý." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình xóa User{ex.Message}" });
            }
        }

    }
}