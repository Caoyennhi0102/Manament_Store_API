using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class UpdatePermissionsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public UpdatePermissionsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: UpdatePermissions
        [HttpGet]
        public ActionResult Index()
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
    }
}