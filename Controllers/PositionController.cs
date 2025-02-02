using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class PositionController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public PositionController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: Position
        public ActionResult Index()
        {
            return View();
        }
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
        [HttpPost]
        public ActionResult SearchPosition(string maCV)
        {
            try
            {
                if (string.IsNullOrEmpty(maCV))
                {
                    return Json(new { success = false, message = "Mã chức vụ không được để trống." });
                }

                var chucvu = _sqlConnectionserver.ChucVus.FirstOrDefault(cv => cv.MaChucVu == maCV);
                if (chucvu == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy chức vụ." });
                }
                return Json(new
                {
                    success = true,
                    MaChucVu = chucvu.MaChucVu,
                    TenCV = chucvu.TenChucVu,
                    MaBP = chucvu.MaBoPhan,
                    tenBoPhan = chucvu.BoPhan?.TenBP


                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình tìm kiếm mã chức vụ{ex.Message}" });
            }
        }
        // GET: AddPosition
        [HttpGet]
        public ActionResult AddPosition()
        {
            var listCV = _sqlConnectionserver.ChucVus.Include(c => c.BoPhan).ToList();
            ViewBag.DSCV = listCV;
            var bophans = _sqlConnectionserver.BoPhans.ToList();
            ViewBag.DSBoPhan = new SelectList(bophans, "MaBoPhan", "TenBP");
            return View();
        }
        [HttpPost]
        public ActionResult AddPosition(string TenCV, string maBP)
        {
            try
            {
                if (string.IsNullOrEmpty(TenCV) || string.IsNullOrEmpty(maBP))
                {
                    return Json(new { success = false, message = "Tên chức vụ và bộ phận không được để trống." });
                }

                var IspostionCode = new Service.PositionCode();
                string MaCV = IspostionCode.CreatePositionCode(TenCV);
                var bophans = _sqlConnectionserver.BoPhans.FirstOrDefault(BP => BP.MaBoPhan == maBP);
                if (bophans == null)
                {
                    return Json(new { success = false, message = "Mã bộ phận không tồn tại" });
                }
                var newChucVu = new ChucVu
                {
                    MaChucVu = MaCV,
                    TenChucVu = TenCV,
                    MaBoPhan = maBP,

                };
                _sqlConnectionserver.ChucVus.Add(newChucVu);
                _sqlConnectionserver.SaveChanges();
                return Json(new
                {
                    success = true,
                    MaChucVu = MaCV,
                    TenChucVu = TenCV,
                    TenBoPhan = bophans.TenBP

                });


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi trong quá trình thêm chức vụ.", error = ex.Message });
            }
        }
        // GET: UpdatePositionDefault
        [HttpGet]
        public ActionResult UpdatePosition()
        {
            var listCV = _sqlConnectionserver.ChucVus.Include(c => c.BoPhan).ToList();
            ViewBag.DSCV = listCV;
            var bophans = _sqlConnectionserver.BoPhans.ToList();
            ViewBag.DSBoPhan = new SelectList(listCV, "MaBoPhan", "TenBoPhan");
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePosition(ChucVu chucVu)
        {
            if (ModelState.IsValid)
            {
                var existingChucVu = _sqlConnectionserver.ChucVus.FirstOrDefault(cv => cv.MaChucVu == chucVu.MaChucVu);
                if (existingChucVu == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy chức vụ cần cập nhật." });
                }
                existingChucVu.TenChucVu = chucVu.TenChucVu;
                existingChucVu.MaBoPhan = chucVu.MaBoPhan;
                _sqlConnectionserver.SaveChanges();

                return Json(new { success = true, message = "Cập nhật chức vụ thành công." });

            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });


        }
        [HttpGet]
        public ActionResult DeletePosition()
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