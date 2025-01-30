using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
   // [Authorize(Roles ="Admin")]
    public class UnlockUserController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public UnlockUserController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: UnlockUser
        [HttpGet]
        public ActionResult Index()
        {
            var users = _sqlConnectionserver.Users.ToList();
            return View(users);
        }
        [HttpPost]
        public ActionResult UnlockUser(int? MaNV)
        {
            try
            {
                if (MaNV == null || MaNV < 0)
                {
                    return Json(new { success = false, message = " Không được để trống hoặc mã nhân viên phải lớn hơn 0 " });

                }
                var user = _sqlConnectionserver.Users.FirstOrDefault(u => u.MaNhanVien == MaNV);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy User tương ứng với mã nhân viên cung cấp " });

                }
                if (user.Locked == false)
                {
                    return Json(new { success = false, message = "User không bị khóa vẫn hoạt động bình thường" });
                }
                user.Locked = false;
                user.TrangThai = "Hoạt Động";
                user.NgayCapNhat = DateTime.Now;
                _sqlConnectionserver.SaveChanges();
                var nhanvien = _sqlConnectionserver.NhanViens.FirstOrDefault(u => u.MaNhanVien == MaNV);
                _sqlConnectionserver.Users.Remove(user);
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
        Nhân viên {user.MaNhanVien} (UserID: {user.UserID}) (Ngày cập nhật:{user.NgayCapNhat}) đã được mở khóa.<br/>
        Vui lòng phê duyệt hoặc từ chối tài khoản tại đường dẫn sau:<br/>
        <a href='https://yourapp.com/User/Approve?UserId={user.UserID}&Action=Approve'>Phê duyệt</a> | 
        <a href='https://yourapp.com/User/Approve?UserId={user.UserID}&Action=Reject'>Từ chối</a>";

                emailService.SendEmail(managerEmail, subject, body);
                return Json(new { success = true, message = "User đã được mở khóa  và email phê duyệt đã được gửi đến quản lý." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi: {ex.Message} xảy ra trong quá trình khóa  User" });
            }
        }

    }
}