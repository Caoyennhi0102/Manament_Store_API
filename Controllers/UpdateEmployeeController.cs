using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class UpdateEmployeeController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public UpdateEmployeeController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: UpdateEmployee
        [HttpGet]
        public ActionResult Index()
        {
            var boPhan = _sqlConnectionserver.BoPhans.ToList();
            ViewBag.DSBoPhan = new SelectList(boPhan, "MaBoPhan", "TenBP");
            ViewBag.DSChucVu = new SelectList(new List<string>(), "");
            return View();
        }
        [HttpPost]
        public ActionResult UpdateEmployee(NhanVien updateNhanVien, int? maNV, string CountryCode)
        {
            try
            {
                if (maNV == null || maNV <= 0)
                {
                    return Json(new { success = false, message = "Mã nhân viên không được để trống hoặc phải lớn hơn 0" });
                }
                var nhanvien = _sqlConnectionserver.NhanViens.FirstOrDefault(u => u.MaNhanVien == maNV);
                if (nhanvien == null)
                {
                    return Json(new { success = false, message = "Mã nhân viên không tồn tại" });
                }
                if (string.IsNullOrEmpty(updateNhanVien.HoTen))
                {
                    return Json(new { success = false, message = "Họ tên không được để trống" });
                }
                if (updateNhanVien.MaCuaHang <= 0)
                {
                    return Json(new { success = false, message = "Mã cửa hàng không được để trống và phải lớn hơn 0" });
                }
                // Kiểm tra mã bộ phận
                if (string.IsNullOrEmpty(updateNhanVien.MaBoPhan))
                {
                    return Json(new { success = false, message = "Mã bộ phận không được để trống" });
                }
                // Kiểm tra mã chức vụ
                if (string.IsNullOrEmpty(updateNhanVien.MaChucVu))
                {
                    return Json(new { success = false, message = "Mã chức vụ không được để trống" });
                }
                // Kiểm tra giới tính
                var validSex = new List<string> { "Nam", "Nữ" };
                if (!validSex.Contains(updateNhanVien.GioiTinh))
                {
                    return Json(new { success = false, message = "Giới tính không hợp lệ" });
                }
                // Kiểm tra ngày sinh
                var IsCheckBirthDay = new Service.CheckBrithDayEmployee();
                if (!IsCheckBirthDay.CheckMinimumAge(updateNhanVien.NgaySinh))
                {
                    return Json(new { success = false, message = "Ngày sinh không họp lệ" });
                }
                //Kiểm tra trình độ học vấn 
                var IsEducational = new List<string> { "Trung Học Phổ Thông", "Đang Chờ Bằng", "Trung Cấp", "Cao Đẳng", "Đại Học" };
                if (!IsEducational.Contains(updateNhanVien.TrinhDoHocVan))
                {
                    return Json(new { success = false, message = "Trình độ học vấn không hợp lệ" });
                }
                // Kiểm tra địa chỉ tạm trú
                var IsAddress = new Service.CheckAddresss();
                if (IsAddress.CheckAddress(updateNhanVien.DiaChiTamtru))
                {
                    return Json(new { success = false, message = "Địa chỉ tạm trú không hợp lệ" });
                }
                // Kiểm tra ngày vào làm
                if (updateNhanVien.NgayVaoLam == default(DateTime) || updateNhanVien.NgayVaoLam > DateTime.Now || updateNhanVien.NgayVaoLam < new DateTime(1900, 1, 1))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Ngày vào làm không hợp lệ! Ngày vào làm không được để trống, không thể là ngày trong tương lai và phải sau năm 1900."
                    });
                }
                var IsStatus = new List<string> { "Hoạt Động", "Không Hoạt Động" };
                if (!IsStatus.Contains(updateNhanVien.TrangThai))
                {
                    return Json(new { success = false, message = "Trạng thái không phù hợp" });
                }
                // Kiểm tra căn cước công dân 
                var iscccd = new Service.Isitizenidentificationnumber();
                if (!iscccd.Checkcitizenidentificationnumber(updateNhanVien.CCCD))
                {
                    return Json(new { success = false, message = "Số căn cước công dân không hợp lệ! Vui lòng nhập đúng định dạng." });
                }

                if (!IsAddress.CheckAddress(updateNhanVien.DiaChiTamtru))
                {
                    return Json(new { success = false, message = "Địa chỉ tạm trú không hợp lệ" });
                }
                var isphone = new Service.CheckPhone();
                string fullPhoneNumber = CountryCode + updateNhanVien.DienThoai;
                bool IsValid = isphone.IsPhoneNumberValid(fullPhoneNumber);
                if (!IsValid)
                {
                    return Json(new { success = false, message = "Số điện thoại không hợp lệ!" });
                }

                var IsCheckAccountBank = new Service.CheckAccountBank();
                if (!IsCheckAccountBank.CheckSalaryAccount(updateNhanVien.TKNhanLuong))
                {
                    return Json(new { success = false, message = "Tài khoản nhận lương không hợp lệ" });
                }
                nhanvien.MaCuaHang = updateNhanVien.MaCuaHang;
                nhanvien.MaBoPhan = updateNhanVien.MaBoPhan;
                nhanvien.MaChucVu = updateNhanVien.MaChucVu;
                nhanvien.HoTen = updateNhanVien.HoTen;
                nhanvien.GioiTinh = updateNhanVien.HoTen;
                nhanvien.NgaySinh = updateNhanVien.NgaySinh;
                nhanvien.CCCD = updateNhanVien.CCCD;
                nhanvien.DiaChi = updateNhanVien.DiaChi;
                nhanvien.DiaChiTamtru = updateNhanVien.DiaChiTamtru;
                nhanvien.DienThoai = fullPhoneNumber;
                nhanvien.NgayVaoLam = updateNhanVien.NgayVaoLam;
                nhanvien.TrinhDoHocVan = updateNhanVien.TrinhDoHocVan;
                var internalEmailService = new Service.InternalEmail();
                string email = internalEmailService.GenerateEmployeeEmail(updateNhanVien.HoTen, (int)maNV);
                nhanvien.Email = email;
                var IsLeave = new Manament_Store_API.Service.EmployeeLeaveDay();
                int annualLeave = IsLeave.CalculateAnnualLeave(updateNhanVien.NgayVaoLam);
                nhanvien.NgayPhepNam = annualLeave;
                var NumberofDaysAllowedtoUseTools = new Service.DaysLeaveUsed();
                int numberOfDay = NumberofDaysAllowedtoUseTools.CalculateUsedLeave((int)maNV);
                nhanvien.SoPhepDaDung = numberOfDay;
                int RemainingLeave = NumberofDaysAllowedtoUseTools.CalculateRemainingLeave(updateNhanVien.NgayPhepNam, (int)maNV);
                nhanvien.SoNgayPhepConLai = RemainingLeave;
                nhanvien.TKNhanLuong = updateNhanVien.TKNhanLuong;
                nhanvien.TrangThai = updateNhanVien.TrangThai;
                nhanvien.TrangThaiDuyetQL = "Chờ Duyệt";

                _sqlConnectionserver.SaveChanges();
                var managerEmail = _sqlConnectionserver.NhanViens.Where(
                    nv => nv.MaChucVu == _sqlConnectionserver.ChucVus.Where(cv => cv.TenChucVu == "Quản lý cửa hàng")
                    .Select(cv => cv.MaChucVu)
                    .FirstOrDefault() && nv.MaBoPhan == updateNhanVien.MaBoPhan)
                    .Select(nv => nv.Email).FirstOrDefault();
                if (string.IsNullOrEmpty(managerEmail))
                {
                    return Json(new { success = true, message = "Nhân viên  đã được tạo nhưng không tìm thấy Quản lý cửa hàng để gửi email phê duyệt." });
                }
                var emailService = new Service.EmailService();
                string subject = "Phê duyệt cập nhật gửi mail";
                string body = $"Xin chào Quản lý,<br/>" +
              $"Nhân viên {updateNhanVien.HoTen} (Mã NV: {updateNhanVien.MaNhanVien}) đã được Admin tạo thêm vào hệ thống.<br/>" +
              $"Vui lòng phê duyệt tài khoản tại đường dẫn sau: " +
              $"<a href='https://yourapp.com/User/Approve?UserId={updateNhanVien.MaNhanVien}&Action=Approve'>Phê duyệt</a> | " +
              $"<a href='https://yourapp.com/User/Approve?UserId={updateNhanVien.MaNhanVien}&Action=Reject'>Từ chối</a>";
                emailService.SendEmail(managerEmail, subject, body);

                return Json(new { success = true, message = "Thêm nhân viên thành công và quản lý phê duyệt" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi:{ex.Message}" });
            }

        }
    }
}