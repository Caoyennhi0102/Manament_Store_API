using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class GetPositionByDepartmentsController : Controller
    {

        private readonly SqlConnectionServer _sqlConnectionserver;

        public GetPositionByDepartmentsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: GetPositionByDepartments
        public ActionResult Index()
        {
            return View();
        }
        //Controller tìm chức vụ theo bộ phận
        [HttpGet]
        public JsonResult GetPositionByDepartments(string maBP)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Mã bộ phận nhận được: {maBP}");
                var chucvu = _sqlConnectionserver.ChucVus.Where(cv => cv.MaBoPhan == maBP)
                    .Select(cv => new { cv.MaChucVu, cv.TenChucVu }).ToList();
                System.Diagnostics.Debug.WriteLine($"Số lượng chức vụ: {chucvu.Count}");
                return Json(chucvu, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi: {ex.Message}");
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}