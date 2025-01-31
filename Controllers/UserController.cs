using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class UserController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;
        public UserController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAllUserList()
        {
            try
            {
                var users = _sqlConnectionserver.Users.Select(u => new
                {
                    u.UserID,
                    u.MaNhanVien,
                    u.Roles,
                    UserName = new string('*', u.UserName.Length), // Hiển thị dấu hoa thị theo độ dài UserName
                    PasswordHash = new string('*', u.PasswordHash.Length), // Hiển thị dấu hoa thị cho Password
                    u.TrangThai,
                    u.MaNVQL,
                    u.TrangThaiDuyetQL,

                }).ToList();
                if (users.Count == 0)
                {
                    return Json(new { success = false, message = "Không có người dùng nào." });
                }
                return Json(users);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }
        [HttpGet]
        public JsonResult SearchUserByMaNV(int MaNV)
        {
            var user = _sqlConnectionserver.Users.FirstOrDefault(u => u.MaNhanVien == MaNV);
            if (user != null)
            {
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        UserID = user.UserID,
                        MaNhanVien = user.MaNhanVien,
                        Roles = user.Roles,
                        TrangThai = user.TrangThai,
                        TrangThaiDuyetQL = user.TrangThaiDuyetQL
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Không tìm thấy User." }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddUser()
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
        // GET: UpdateUser
        [HttpGet]
        public ActionResult UpdateUser()
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
        [HttpGet]
        public ActionResult DeleteUser()
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
        // GET: LockUser
        [HttpGet]
        public ActionResult LockUser()
        {
            var users = _sqlConnectionserver.Users.ToList();
            return View(users);
        }
        [HttpPost]
        public ActionResult LockUser(int? MaNV)
        {
            try
            {
                if (MaNV == null || MaNV < 0)
                {
                    return Json(new { success = false, message = "Không được để trống hoặc mã nhân phải lớn hơn 0" });
                }
                var user = _sqlConnectionserver.Users.FirstOrDefault(u => u.MaNhanVien == MaNV);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy User tương ứng với mã nhân viên cung cấp " });

                }
                if (user.Locked == false)
                {
                    return Json(new { success = false, message = "User đã bị khóa trước đó" });
                }
                user.TrangThai = "Khóa";
                user.Locked = true;
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
        Nhân viên {user.MaNhanVien} (UserID: {user.UserID}) đã được khóa  .<br/>
        Vui lòng phê duyệt hoặc từ chối tài khoản tại đường dẫn sau:<br/>
        <a href='https://yourapp.com/User/Approve?UserId={user.UserID}&Action=Approve'>Phê duyệt</a> | 
        <a href='https://yourapp.com/User/Approve?UserId={user.UserID}&Action=Reject'>Từ chối</a>";

                emailService.SendEmail(managerEmail, subject, body);
                return Json(new { success = true, message = "User đã được khóa  và email phê duyệt đã được gửi đến quản lý." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình khóa User Lỗi:{ex.Message}" });
            }
        }
        [HttpGet]
        public ActionResult UnlockUser()
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