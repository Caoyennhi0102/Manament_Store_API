using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class GetAllEmployeeController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public GetAllEmployeeController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: GetAllEmployee
        public ActionResult Index()
        {
            return View();
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
    }
}