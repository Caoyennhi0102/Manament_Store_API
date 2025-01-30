using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class SearchEmployeeCodeController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public SearchEmployeeCodeController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: SearchEmployeeCode
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
    }
}