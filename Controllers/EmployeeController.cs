using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public EmployeeController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SearchEmployeeCode(int? MaNV)
        {
            try
            {
                if (MaNV == null || MaNV <= 0)
                {
                    return Json(new { success = false, message = "Mã nhân viên không được để trống hoặc phải lớn hơn 0" });
                }
                var nhanvien = _sqlConnectionserver.NhanViens.FirstOrDefault(u => u.MaNhanVien == MaNV);
                if (nhanvien != null)
                {
                    return Json(new
                    {
                        success = true,
                        data = new
                        {
                            maNV = nhanvien.MaNhanVien,
                            maCuaHang = nhanvien.MaCuaHang,
                            maBoPhan = nhanvien.MaBoPhan,
                            maChucVu = nhanvien.MaChucVu,
                            hoTen = nhanvien.HoTen,
                            ngaySinh = nhanvien.NgaySinh,
                            gioiTinh = nhanvien.GioiTinh,
                            cccd = nhanvien.CCCD,
                            diachiThuongTru = nhanvien.DiaChi,
                            diachitamTru = nhanvien.DiaChiTamtru,
                            email = nhanvien.Email,
                            dienThoai = nhanvien.DienThoai,
                            ngayvaolam = nhanvien.NgayVaoLam,
                            ngayphepnam = nhanvien.NgayPhepNam,
                            sophepSuDung = nhanvien.SoPhepDaDung,
                            sophepConLai = nhanvien.SoNgayPhepConLai,
                            trinhdoHocVan = nhanvien.TrinhDoHocVan,
                            taikhoanNhanLuong = nhanvien.TKNhanLuong,
                            trangThai = nhanvien.TrangThai,
                            trangThaiDuyetQL = nhanvien.TrangThaiDuyetQL,
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "Mã nhân viên không tồn tại" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi:{ex.Message}" });
            }
        }
        [HttpGet]
        public JsonResult GetAllEmployee()
        {
            try
            {
                var nhanvien = _sqlConnectionserver.NhanViens.Select(u => new
                {
                    u.MaNhanVien,
                    u.MaCuaHang,
                    u.MaBoPhan,
                    u.MaChucVu,
                    u.HoTen,
                    NgaySinh = u.NgaySinh.ToString("yyyy-MM-dd"),
                    u.GioiTinh,
                    u.CCCD,
                    u.DienThoai,
                    u.DiaChi,
                    u.DiaChiTamtru,
                    u.Email,
                    NgayVaoLam = u.NgayVaoLam.ToString("yyyy-MM-dd"), // Định dạng ngày
                    u.TrinhDoHocVan,
                    u.NgayPhepNam,
                    u.SoPhepDaDung,
                    u.SoNgayPhepConLai,
                    u.TrangThai,
                    u.TrangThaiDuyetQL,

                }).ToList();
                if (nhanvien.Count == 0)
                {
                    return Json(new { success = false, message = "Không có nhân viên  nào." });
                }
                return Json(new { success = true, data = nhanvien });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
        public ActionResult AddEmployee()
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
        // GET: UpdateEmployee
        [HttpGet]
        public ActionResult UpdateEmployee()
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
        // GET: DeleteEmployee
        [HttpGet]
        public ActionResult DeleteEmployee()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteEmployee(int? MaNV)
        {
            try
            {
                if (MaNV == null || MaNV < 0)
                {
                    return Json(new { success = false, message = "Mã nhân viên không được để trống hoặc phải lớn hơn 0" });
                }
                var deletenhanvien = _sqlConnectionserver.NhanViens.Find(MaNV);
                if (deletenhanvien != null)
                {
                    var deleteNVuser = _sqlConnectionserver.Users.Where(u => u.MaNhanVien == MaNV);
                    if (deleteNVuser.Any())
                    {
                        _sqlConnectionserver.Users.RemoveRange(deleteNVuser);
                    }
                    var deleteNVCHT = _sqlConnectionserver.CuaHangs.Where(u => u.CHTruong == MaNV);
                    if (deleteNVCHT.Any())
                    {
                        _sqlConnectionserver.CuaHangs.RemoveRange(deleteNVCHT);
                    }
                    var deleteNVBac = _sqlConnectionserver.BacLuongs.Where(u => u.MaNhanVien == MaNV);
                    if (deleteNVBac.Any())
                    {
                        _sqlConnectionserver.BacLuongs.RemoveRange(deleteNVBac);
                    }
                    var deleteNvHoaDon = _sqlConnectionserver.HoaDons.Where(u => u.MaNhanVien == MaNV);
                    if (deleteNvHoaDon.Any())
                    {
                        _sqlConnectionserver.HoaDons.RemoveRange(deleteNvHoaDon);
                    }
                    var deleteNvTK = _sqlConnectionserver.ThongKeBanHangs.Where(u => u.MaNhanVien == MaNV);
                    if (deleteNvTK.Any())
                    {
                        _sqlConnectionserver.ThongKeBanHangs.RemoveRange(deleteNvTK);
                    }
                    var deletePTH = _sqlConnectionserver.PhieuTraHangs.Where(u => u.MaNhanVien == MaNV);
                    if (deletePTH.Any())
                    {
                        _sqlConnectionserver.PhieuTraHangs.RemoveRange(deletePTH);
                    }
                    var deletePNH = _sqlConnectionserver.PhieuNhapHangs.Where(u => u.MaNhanVien == MaNV);
                    if (deletePNH.Any())
                    {
                        _sqlConnectionserver.PhieuNhapHangs.RemoveRange(deletePNH);
                    }
                    var deleteNP = _sqlConnectionserver.YeuCauNghiPheps.Where(u => u.MaNhanVien == MaNV);
                    if (deleteNP.Any())
                    {
                        _sqlConnectionserver.YeuCauNghiPheps.RemoveRange(deleteNP);
                    }
                    var deteleNVQLC = _sqlConnectionserver.QLCaNVs.Where(u => u.MaNhanVien == MaNV);
                    if (deteleNVQLC.Any())
                    {
                        _sqlConnectionserver.QLCaNVs.RemoveRange(deteleNVQLC);
                    }
                    var deleteNVCong = _sqlConnectionserver.Congs.Where(u => u.MaNhanVien == MaNV);
                    if (deleteNVCong.Any())
                    {
                        _sqlConnectionserver.Congs.RemoveRange(deleteNVCong);
                    }
                    var deleteNVKPI = _sqlConnectionserver.KPIs.Where(u => u.MaNhanVien == MaNV);
                    if (deleteNVKPI.Any())
                    {
                        _sqlConnectionserver.KPIs.RemoveRange(deleteNVKPI);
                    }
                    var deleteNVLuong = _sqlConnectionserver.Luongs.Where(u => u.MaNhanVien == MaNV);
                    if (deleteNVLuong.Any())
                    {
                        _sqlConnectionserver.Luongs.RemoveRange(deleteNVLuong);
                    }
                    var deleteNVHDLD = _sqlConnectionserver.HDLaoDongs.Where(u => u.MaNhanVien == MaNV);
                    if (deleteNVHDLD.Any())
                    {
                        _sqlConnectionserver.HDLaoDongs.RemoveRange(deleteNVHDLD);
                    }
                    deletenhanvien.TrangThai = "Đã thôi việc";
                    deletenhanvien.TrangThaiDuyetQL = "Chờ Duyệt";
                    _sqlConnectionserver.NhanViens.Remove(deletenhanvien);
                    _sqlConnectionserver.SaveChanges();
                    var managerEmail = _sqlConnectionserver.NhanViens.Where(
                        nv => nv.MaChucVu == _sqlConnectionserver.ChucVus.Where(cv => cv.TenChucVu == "Quản lý cửa hàng")
                        .Select(cv => cv.MaChucVu)
                        .FirstOrDefault() && nv.MaBoPhan == deletenhanvien.MaBoPhan)
                        .Select(nv => nv.Email).FirstOrDefault();
                    if (string.IsNullOrEmpty(managerEmail))
                    {
                        return Json(new { success = true, message = "Nhân viên  đã được tạo nhưng không tìm thấy Quản lý cửa hàng để gửi email phê duyệt." });
                    }
                    var emailService = new Service.EmailService();
                    string subject = "Phê duyệt cập nhật gửi mail";
                    string body = $"Xin chào Quản lý,<br/>" +
                  $"Nhân viên {deletenhanvien.HoTen} (Mã NV: {deletenhanvien.MaNhanVien}) đã được Admin tạo thêm vào hệ thống.<br/>" +
                  $"Vui lòng phê duyệt tài khoản tại đường dẫn sau: " +
                  $"<a href='https://yourapp.com/User/Approve?UserId={deletenhanvien.MaNhanVien}&Action=Approve'>Phê duyệt</a> | " +
                  $"<a href='https://yourapp.com/User/Approve?UserId={deletenhanvien.MaNhanVien}&Action=Reject'>Từ chối</a>";
                    emailService.SendEmail(managerEmail, subject, body);
                    return Json(new { success = true, message = "Nhân viên đã xóa thành công và được gửi cho quản lý phê duyệt" });
                }
                return Json(new { success = false, message = "Xóa không thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi:{ex.Message}" });
            }
        }

    }
}