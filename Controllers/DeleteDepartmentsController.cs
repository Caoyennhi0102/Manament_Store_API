using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class DeleteDepartmentsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public DeleteDepartmentsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: DeleteDepartments
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        //Phương thức xóa  bộ phận
        [HttpPost]
        public ActionResult DeleteDepartments(string maBP)
        {
            try
            {
                if (string.IsNullOrEmpty(maBP))
                {
                    return Json(new { success = false, message = "Mã bộ phận không được để trống." });

                }
                var bophans = _sqlConnectionserver.BoPhans.Find(maBP);
                if (bophans != null)
                {
                    var deleteBPNV = _sqlConnectionserver.NhanViens.Where(u => u.MaBoPhan == maBP);
                    if (deleteBPNV.Any())
                    {
                        _sqlConnectionserver.NhanViens.RemoveRange(deleteBPNV);

                    }
                    var deleteBPCV = _sqlConnectionserver.ChucVus.Where(u => u.MaBoPhan == maBP);
                    if (deleteBPNV.Any())
                    {
                        _sqlConnectionserver.ChucVus.RemoveRange(deleteBPCV);
                    }
                    _sqlConnectionserver.BoPhans.Remove(bophans);
                    _sqlConnectionserver.SaveChanges();
                    return Json(new { success = true, message = "Xóa bộ phận thành công" });


                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy bộ phận với mã đã cho." });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = $"Có lỗi xảy ra trong quá trình xóa bộ phận {ex.Message}" });
            }
        }

    }
}