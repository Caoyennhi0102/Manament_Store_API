using Manament_Store_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class RevokePermissionsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public RevokePermissionsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: RevokePermissions
        [HttpGet]
        public ActionResult Index()
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