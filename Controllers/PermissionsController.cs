using Manament_Store_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class PermissionsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public PermissionsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: Permissions
        public ActionResult Index()
        {
            return View();
        }
        // GET: CreatePermissions
        [HttpGet]
        public ActionResult CreatePermissions()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchCodePermissions(string maQuyen)
        {
            try
            {
                if (string.IsNullOrEmpty(maQuyen))
                {
                    return Json(new { success = false, message = "Mã quyền không được để trống." });
                }

                if (_sqlConnectionserver == null)
                {
                    return Json(new { success = false, message = "Kết nối cơ sở dữ liệu không khả dụng." });
                }

                var quyen = _sqlConnectionserver.Quyens.FirstOrDefault(u => u.MaQuyen == maQuyen);

                if (quyen != null)
                {
                    return Json(new { success = true, data = quyen });
                }

                return Json(new { success = false, message = "Không tìm thấy quyền!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }
        [HttpGet]
        public JsonResult GetPermissions()
        {
            try
            {
                var ListPermissions = _sqlConnectionserver.Quyens.Select(q => new
                {
                    q.MaQuyen,
                    q.TenQuyen
                }).ToList();
                return Json(ListPermissions, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Đã xảy ra lỗi: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult CreatePermissions(string tenQuyen)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tenQuyen))
                {
                    return Json(new { success = false, message = "Tên quyền không được để trống!" });
                }
                var IsVaild = new Service.CodePermissions();
                string maQuyen = IsVaild.CreateCodePermissions(tenQuyen);
                if (_sqlConnectionserver.Quyens.Any(q => q.MaQuyen == maQuyen))
                {
                    return Json(new { success = false, message = "Mã quyền đã tồn tại!" });
                }
                var quyen = new Quyen
                {
                    MaQuyen = maQuyen,
                    TenQuyen = tenQuyen
                };
                _sqlConnectionserver.Quyens.Add(quyen);
                _sqlConnectionserver.SaveChanges();
                return Json(new { success = true, message = "Tạo quyền thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi:{ex.Message} xảy ra trong quá trình tạo quyền" });
            }

        }
        [HttpGet]
        public ActionResult UpdatePermissions()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdatePermissions(string maQuyen, string tenQuyen)
        {
            try
            {
                if (string.IsNullOrEmpty(maQuyen))
                {
                    return Json(new { success = false, message = "Mã quyền không được để trống" });
                }
                var quyen = _sqlConnectionserver.Quyens.FirstOrDefault(u => u.MaQuyen == maQuyen);
                if (quyen == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy quyền với mã đã cung cấp" });
                }
                if (string.IsNullOrEmpty(tenQuyen))
                {
                    quyen.TenQuyen = tenQuyen;
                    _sqlConnectionserver.SaveChanges();
                    return Json(new { success = true, message = "Cập nhật quyền thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Tên quyền không được để trống" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = $"Có lỗi:{ex.Message} trong quá trình cập nhật quyền" });
            }


        }
        [HttpGet]
        public ActionResult DeletePermissions()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeletePermissions(string maQuyen)
        {
            if (string.IsNullOrEmpty(maQuyen))
            {
                return Json(new { success = false, message = "Mã quyền không được để trống" });
            }
            var quyen = _sqlConnectionserver.Quyens.Find(maQuyen);
            if (quyen != null)
            {
                var maquyen = _sqlConnectionserver.Users.Where(u => u.MaQuyen == maQuyen);
                if (maquyen.Any())
                {
                    _sqlConnectionserver.Users.RemoveRange(maquyen);
                }
                _sqlConnectionserver.Quyens.Remove(quyen);
                _sqlConnectionserver.SaveChanges();
                return Json(new { success = true, message = "Xóa quyền thành công" });
            }
            return Json(new { success = false, message = "Mã quyền không tồn tại" });
        }
        [HttpGet]
        public ActionResult GrantingPermissions()
        {
            var listPermissions = _sqlConnectionserver.Quyens.ToList();
            ViewBag.DSQuyen = new SelectList(listPermissions, "MaQuyen", "TenQuyen");
            return View();
        }
        [HttpPost]
        public ActionResult GrantingPermissions(int? MaNV, List<string> selectedPermissions)
        {
            try
            {
                var user = _sqlConnectionserver.Users.FirstOrDefault(u => u.MaNhanVien == MaNV);
                if (user != null)
                {
                    return Json(new { success = false, message = "Không tìm thấy User." });
                }
                if (selectedPermissions != null && selectedPermissions.Any())
                {
                    user.MaQuyen = string.Join(",", selectedPermissions);
                }
                else
                {
                    user.MaQuyen = null;
                }
                user.NgayCapNhat = DateTime.Now;
                _sqlConnectionserver.SaveChanges();

                return Json(new { success = true, message = "Cấp quyền thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
        [HttpGet]
        public ActionResult RevokePermissions()
        {
            var listPermissions = _sqlConnectionserver.Quyens.ToList();
            ViewBag.DSQuyen = JsonConvert.SerializeObject(listPermissions);
            return View();
        }
        [HttpPost]
        public ActionResult RevokePermissions(int? MaNV, List<string> selectedPermissions)
        {
            try
            {
                var user = _sqlConnectionserver.Users.FirstOrDefault(u => u.MaNhanVien == MaNV);
                if (user == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy User." });
                }
                if (selectedPermissions != null && selectedPermissions.Any())
                {
                    var currentPermissions = user.MaQuyen?.Split(',').ToList() ?? new List<string>();
                    foreach (var permission in selectedPermissions)
                    {
                        currentPermissions.Remove(permission);
                    }
                    user.MaQuyen = currentPermissions.Any() ? string.Join(",", currentPermissions) : null;

                }
                else
                {
                    user.MaQuyen = null;
                }
                user.NgayCapNhat = DateTime.Now;
                _sqlConnectionserver.SaveChanges();
                return Json(new { success = true, message = "Thu quyền thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
    }
}