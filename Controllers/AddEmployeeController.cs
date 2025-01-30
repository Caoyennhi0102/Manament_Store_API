using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    //[Authorize(Roles ="Admin")]

    public class AddEmployeeController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public AddEmployeeController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: AddEmployee
        public ActionResult Index()
        {
            var boPhan = _sqlConnectionserver.BoPhans.ToList();
            ViewBag.DSBoPhan = new SelectList(boPhan, "MaBoPhan", "TenBP");
            ViewBag.DSChucVu = new SelectList(new List<string>(), "");
            return View();
            
        }
        // Controller thêm nhân viên 
        [HttpPost]
        public ActionResult AddEmployee(NhanVien nhanVien, int? macuahang, string MaBoPhan, string MaChucVu, string hoTen, string gioiTinh, DateTime NgaySinh, string cccd, string diaChi, string email, string dienThoai, DateTime NgayVaoLam, string taiKhoanLuong, string trinhdoHocVan, string diachiTamTru, string trangthai, string CountryCode)
        {
            try
            {
                if (string.IsNullOrEmpty(hoTen))
                {
                    return Json(new { success = false, message = "Họ tên không được để trống" });
                }
                if (macuahang == null || macuahang <= 0)
                {
                    return Json(new { success = false, message = "Mã cửa hàng không được để trống và phải lớn hơn 0" });
                }
                // Kiểm tra mã bộ phận
                if (string.IsNullOrEmpty(MaBoPhan))
                {
                    return Json(new { success = false, message = "Mã bộ phận không được để trống" });
                }
                // Kiểm tra mã chức vụ
                if (string.IsNullOrEmpty(MaChucVu))
                {
                    return Json(new { success = false, message = "Mã chức vụ không được để trống" });
                }
                // Kiểm tra giới tính
                var validSex = new List<string> { "Nam", "Nữ" };
                if (!validSex.Contains(gioiTinh))
                {
                    return Json(new { success = false, message = "Giới tính không hợp lệ" });
                }
                // Kiểm tra ngày sinh
                var IsCheckBirthDay = new Service.CheckBrithDayEmployee();
                if (!IsCheckBirthDay.CheckMinimumAge(NgaySinh))
                {
                    return Json(new { success = false, message = "Ngày sinh không họp lệ" });
                }
                //Kiểm tra trình độ học vấn 
                var IsEducational = new List<string> { "Trung Học Phổ Thông", "Đang Chờ Bằng", "Trung Cấp", "Cao Đẳng", "Đại Học" };
                if (!IsEducational.Contains(trinhdoHocVan))
                {
                    return Json(new { success = false, message = "Trình độ học vấn không hợp lệ" });
                }
                // Kiểm tra địa chỉ tạm trú
                var IsAddress = new Service.CheckAddresss();
                if (!IsAddress.CheckAddress(diachiTamTru))
                {
                    return Json(new { success = false, message = "Địa chỉ tạm trú không hợp lệ" });
                }
                // Kiểm tra ngày vào làm
                if (NgayVaoLam == default(DateTime) || NgayVaoLam > DateTime.Now || NgayVaoLam < new DateTime(1900, 1, 1))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Ngày vào làm không hợp lệ! Ngày vào làm không được để trống, không thể là ngày trong tương lai và phải sau năm 1900."
                    });
                }
                var IsStatus = new List<string> { "Hoạt Động", "Không Hoạt Động" };
                if (!IsStatus.Contains(trangthai))
                {
                    return Json(new { success = false, message = "Trạng thái không phù hợp" });
                }
                // Kiểm tra căn cước công dân 
                var iscccd = new Service.Isitizenidentificationnumber();
                if (!iscccd.Checkcitizenidentificationnumber(cccd))
                {
                    return Json(new { success = false, message = "Số căn cước công dân không hợp lệ! Vui lòng nhập đúng định dạng." });
                }

                if (!IsAddress.CheckAddress(diaChi))
                {
                    return Json(new { success = false, message = "Địa chỉ tạm trú không hợp lệ" });
                }
                var isphone = new Service.CheckPhone();
                string fullPhoneNumber = CountryCode + dienThoai;
                bool IsValid = isphone.IsPhoneNumberValid(fullPhoneNumber);
                if (!IsValid)
                {
                    return Json(new { success = false, message = "Số điện thoại không hợp lệ!" });
                }

                var IsCheckAccountBank = new Service.CheckAccountBank();
                if (!IsCheckAccountBank.CheckSalaryAccount(taiKhoanLuong))
                {
                    return Json(new { success = false, message = "Tài khoản nhận lương không hợp lệ" });
                }
                var IsEmployeeID = new Service.EmployeeID();
                int manhanvien = IsEmployeeID.GenerateEmployeeID();
                nhanVien.MaNhanVien = manhanvien;
                nhanVien.MaCuaHang = (int)macuahang;
                nhanVien.MaBoPhan = MaBoPhan;
                nhanVien.MaChucVu = MaChucVu;
                nhanVien.HoTen = hoTen;
                nhanVien.GioiTinh = gioiTinh;
                nhanVien.NgaySinh = NgaySinh;
                nhanVien.CCCD = cccd;
                nhanVien.DiaChi = diaChi;
                nhanVien.DiaChiTamtru = diachiTamTru;
                nhanVien.DienThoai = fullPhoneNumber;
                nhanVien.NgayVaoLam = NgayVaoLam;
                nhanVien.TrinhDoHocVan = trinhdoHocVan;
                var internalEmailService = new Service.InternalEmail();
                email = internalEmailService.GenerateEmployeeEmail(hoTen, manhanvien);
                nhanVien.Email = email;

                var IsLeave = new Manament_Store_API.Service.EmployeeLeaveDay();
                int annualLeave = IsLeave.CalculateAnnualLeave(NgayVaoLam);
                nhanVien.NgayPhepNam = annualLeave;
                var NumberofDaysAllowedtoUseTools = new Service.DaysLeaveUsed();
                int numberOfDay = NumberofDaysAllowedtoUseTools.CalculateUsedLeave(manhanvien);
                nhanVien.SoPhepDaDung = numberOfDay;
                int RemainingLeave = NumberofDaysAllowedtoUseTools.CalculateRemainingLeave(nhanVien.NgayPhepNam, manhanvien);
                nhanVien.SoNgayPhepConLai = RemainingLeave;
                nhanVien.TKNhanLuong = taiKhoanLuong;
                nhanVien.TrangThai = trangthai;
                nhanVien.TrangThaiDuyetQL = "Chờ Duyệt";
                _sqlConnectionserver.NhanViens.Add(nhanVien);
                _sqlConnectionserver.SaveChanges();
                var managerEmail = _sqlConnectionserver.NhanViens.Where(
                    nv => nv.MaChucVu == _sqlConnectionserver.ChucVus.Where(cv => cv.TenChucVu == "Quản lý cửa hàng")
                    .Select(cv => cv.MaChucVu)
                    .FirstOrDefault() && nv.MaBoPhan == nhanVien.MaBoPhan)
                    .Select(nv => nv.Email).FirstOrDefault();
                if (string.IsNullOrEmpty(managerEmail))
                {
                    return Json(new { success = true, message = "Nhân viên  đã được tạo nhưng không tìm thấy Quản lý cửa hàng để gửi email phê duyệt." });
                }
                var emailService = new Service.EmailService();
                string subject = "Phê duyệt nhân viên  mới";
                string body = $"Xin chào Quản lý,<br/>" +
              $"Nhân viên {nhanVien.HoTen} (Mã NV: {nhanVien.MaNhanVien}) đã được Admin tạo thêm vào hệ thống.<br/>" +
              $"Vui lòng phê duyệt tài khoản tại đường dẫn sau: " +
              $"<a href='https://yourapp.com/User/Approve?UserId={nhanVien.MaNhanVien}&Action=Approve'>Phê duyệt</a> | " +
              $"<a href='https://yourapp.com/User/Approve?UserId={nhanVien.MaNhanVien}&Action=Reject'>Từ chối</a>";
                emailService.SendEmail(managerEmail, subject, body);

                return Json(new { success = true, message = "Thêm nhân viên thành công và quản lý phê duyệt" });


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi : {ex.Message}" });
            }
        }


    }
}