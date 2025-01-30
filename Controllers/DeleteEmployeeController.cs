using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class DeleteEmployeeController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public DeleteEmployeeController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: DeleteEmployee
        [HttpGet]
        public ActionResult Index()
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