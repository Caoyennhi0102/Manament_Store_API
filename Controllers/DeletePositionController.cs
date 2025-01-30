using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class DeletePositionController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public DeletePositionController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: DeletePosition
        [HttpGet]
        public ActionResult Index()
        {
            var listCV = _sqlConnectionserver.ChucVus.Include(c => c.BoPhan).ToList();
            ViewBag.DSCV = listCV;
            var bophans = _sqlConnectionserver.BoPhans.ToList();
            ViewBag.DSBoPhan = new SelectList(listCV, "MaBoPhan", "TenBoPhan");
            return View();
        }
        
        [HttpPost]
        public ActionResult DeletePosition(string MaCV)
        {
            try
            {
                if (string.IsNullOrEmpty(MaCV))
                {
                    return Json(new { success = false, message = "Mã chức vụ không được để trống." });
                }
                var deleteCV = _sqlConnectionserver.ChucVus.Find(MaCV);
                if (deleteCV != null)
                {
                    var deletePositionNV = _sqlConnectionserver.NhanViens.Where(u => u.MaChucVu == MaCV);
                    if (deletePositionNV.Any())
                    {
                        _sqlConnectionserver.NhanViens.RemoveRange(deletePositionNV);
                    }
                    _sqlConnectionserver.ChucVus.Remove(deleteCV);
                    _sqlConnectionserver.SaveChanges();
                    return Json(new { success = true, message = "Xóa chức vụ thành công" });
                }
                return Json(new { success = false, message = "Mã chức vụ không tồn tại" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình xóa chức vụ{ex.Message}" });
            }
        }
    }
}