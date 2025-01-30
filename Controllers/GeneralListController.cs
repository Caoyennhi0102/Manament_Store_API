using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class GeneralListController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public GeneralListController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: GeneralList
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GeneralList()
        {
            return View();
        }
        // POST: /Admin/GeneralList (Xử lý AJAX)
        [HttpPost]
        public JsonResult GeneralList(string filter)
        {
            try
            {
                // Lấy danh sách nhân viên, user và cửa hàng từ cơ sở dữ liệu
                var employees = _sqlConnectionServer.NhanViens.ToList();
                var users = _sqlConnectionServer.Users.ToList();
                var stores = _sqlConnectionServer.CuaHangs.ToList();
                if (!employees.Any() || !stores.Any() || !users.Any())
                {
                    return Json(new { success = false, message = "Không có dữ liệu trong cơ sở dữ liệu." });
                }
                // Áp dụng bộ lọc dựa trên giá trị của filter
                switch (filter)
                {
                    case "hasUser":
                        employees = employees.Where(e => users.Any(u => u.MaNhanVien == e.MaNhanVien)).ToList();
                        break;
                    case "noUser":
                        employees = employees.Where(e => !users.Any(u => u.MaNhanVien == e.MaNhanVien)).ToList();
                        break;
                    case "activeEmployees":
                        employees = employees.Where(e => e.TrangThai == "Đang Làm Việc").ToList();
                        break;
                    case "inactiveEmployees":
                        employees = employees.Where(e => e.TrangThai == "Nghỉ Việc").ToList();
                        break;
                    case "loginHistory":
                        users = users.Where(u => u.TGDangNhap != null).ToList();
                        break;
                    case "logOutHistory":
                        users = users.Where(u => u.TGDangXuat != null).ToList();
                        break;
                    case "allStores":
                        // Không cần lọc, lấy tất cả cửa hàng
                        break;
                    case "storeManagers":
                        employees = employees.Where(e => stores.Any(s => e.ChucVu.TenChucVu == "Quản Lý")).ToList();
                        break;
                    case "closedStores":
                        stores = stores.Where(s => s.TrangThai == "Đã Đóng Cửa").ToList();
                        break;
                    case "openStores":
                        stores = stores.Where(s => s.TrangThai == "Đang Hoạt Động").ToList();
                        break;
                    default:
                        return Json(new { success = false, message = "Bộ lọc không hợp lệ." });
                }

                // Tạo kết quả trả về
                var result = new
                {
                    success = true,
                    employees = employees.Select(e => new { e.MaNhanVien, TenCuaHang = e.CuaHang.TenCH, e.HoTen, e.GioiTinh, e.NgaySinh, e.TrangThai }),
                    stores = stores.Select(s => new { s.MaCuaHang, s.TenCH, s.DiaChi, s.TrangThai }),
                    users = users.Select(s => new { s.UserID, s.MaNhanVien, TenNhanVien = s.NhanVien.HoTen, s.TGDangNhap, s.TGDangXuat, s.TrangThai })
                };

                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
    }
}