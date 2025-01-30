using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{

    public class AddUserController : Controller
    {

        private readonly SqlConnectionServer _sqlConnectionserver;

        public AddUserController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: AddUser
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(int? MaNV, string role)
        {
            try
            {
                if (MaNV == null || MaNV <= 0)
                {
                    return Json(new { success = false, message = "Vui lòng nhập mã nhân viên hợp lệ" });
                }
                var nhanvien = _sqlConnectionserver.NhanViens.FirstOrDefault(u => u.MaNhanVien == MaNV);
                if (nhanvien == null)
                {
                    return Json(new { success = false, message = "Mã nhân viên không tồn tại." });
                }
                var existingUser = _sqlConnectionserver.Users.FirstOrDefault(u => u.MaNhanVien == MaNV);
                if (existingUser != null)
                {
                    return Json(new { success = false, message = "Nhân viên đã có tài khoản." });
                }
                var validRoles = new List<string> { "Admin", "Manager", "User" };
                if (!validRoles.Contains(role))
                {
                    return Json(new { success = false, message = "Vai trò không hợp lệ." });
                }
                var IsUserName = new Service.UserNameRandom();
                string username = "User_" + MaNV + "_" + IsUserName.GenerateRandomString(5);
                string password = IsUserName.GenerateRandomString(10);
                var Ispassword = new Service.PasswordRandom();
                var passwordHashAndSalt = Ispassword.HashPassword(password);
                var newUser = new User
                {
                    MaNhanVien = nhanvien.MaNhanVien,
                    UserName = username,
                    PasswordSalt = passwordHashAndSalt.Salt,
                    PasswordHash = passwordHashAndSalt.HashedPassword,
                    TGDangNhap = null,
                    TGDangXuat = null,
                    Locked = false,
                    DiaChiIP = null,
                    DNLanDau = true,
                    Roles = role,
                    TrangThai = "Chờ Duyệt",
                    TrangThaiDuyetQL = "Chờ Duyệt"

                };
                _sqlConnectionserver.Users.Add(newUser);
                _sqlConnectionserver.SaveChanges();
                var managerEmail = _sqlConnectionserver.NhanViens.Where(
                    nv => nv.MaChucVu == _sqlConnectionserver.ChucVus.Where(cv => cv.TenChucVu == "Quản lý cửa hàng")
                    .Select(cv => cv.MaChucVu)
                    .FirstOrDefault() && nv.MaBoPhan == nhanvien.MaBoPhan)
                    .Select(nv => nv.Email).FirstOrDefault();
                if (string.IsNullOrEmpty(managerEmail))
                {
                    return Json(new { success = true, message = "User đã được tạo nhưng không tìm thấy Quản lý cửa hàng để gửi email phê duyệt." });
                }
                var emailService = new Service.EmailService();
                string subject = "Phê duyệt User mới";
                string body = $"Xin chào Quản lý,<br/>" +
              $"Nhân viên {nhanvien.HoTen} (Mã NV: {MaNV}) đã được Admin tạo tài khoản.<br/>" +
              $"Vui lòng phê duyệt tài khoản tại đường dẫn sau: " +
              $"<a href='https://yourapp.com/User/Approve?UserId={newUser.UserID}&Action=Approve'>Phê duyệt</a> | " +
              $"<a href='https://yourapp.com/User/Approve?UserId={newUser.UserID}&Action=Reject'>Từ chối</a>";
                emailService.SendEmail(managerEmail, subject, body);

                return Json(new { success = true, message = " User đã được tạo và đang chờ phê duyệt." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }
    }
}