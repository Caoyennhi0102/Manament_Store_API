using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class CreatePermissionsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public CreatePermissionsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: CreatePermissions
        [HttpGet]
        public ActionResult Index()
        {
            return View();
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
    }
}