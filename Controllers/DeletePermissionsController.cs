using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DeletePermissionsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public DeletePermissionsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: DeletePermissions
        [HttpGet]
        public ActionResult Index()
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
    }
}