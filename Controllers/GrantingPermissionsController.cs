using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class GrantingPermissionsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public GrantingPermissionsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: GrantingPermissions
        [HttpGet]
        public ActionResult Index()
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
    }
}