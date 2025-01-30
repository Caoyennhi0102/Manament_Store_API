using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
  //  [Authorize(Roles ="Admin")]
    public class CreateDepartmentsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public CreateDepartmentsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: CreateDepartments
        [HttpGet]
        public ActionResult Index()
        {
            var store = _sqlConnectionserver.CuaHangs.Select(ch => new { ch.MaCuaHang, ch.TenCH }).ToList();
            ViewBag.Store = new SelectList(store, "MaCuaHang", "TenCH");
            return View();
        }
        // POST: CreateDepartments
        [HttpPost]
        public ActionResult CreateDepartments(string TenBP, int? maCH)
        {
            try
            {
                if (string.IsNullOrEmpty(TenBP) || maCH == null)
                {
                    return Json(new { success = false, message = "Tên bộ phận và mã cửa hàng không được để trống." });

                }
                var IsDepartments = new Service.DepartmentsCode();
                string MaBP = IsDepartments.CreateCodeDepartments(TenBP);
                var department = new BoPhan
                {
                    MaBoPhan = MaBP,
                    TenBP = TenBP,
                    MaCuaHang = (int)maCH,
                };
                _sqlConnectionserver.BoPhans.Add(department);
                _sqlConnectionserver.SaveChanges();
                return Json(new { success = true, message = "Thêm bộ phận thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình thêm bộ phận{ex.Message}" });
            }
        }
    }
}