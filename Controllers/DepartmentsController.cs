using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public DepartmentsController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: Departments
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetDepartments()
        {
            try
            {
                var departments = _sqlConnectionserver.BoPhans.Select(
                    bp => new
                    {
                        bp.MaBoPhan,
                        bp.TenBP,
                        TenCuaHang = bp.CuaHang.TenCH
                    })
                    .ToList();
                return Json(departments, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra khi tải danh sách bộ phận: {ex.Message}" });
            }
        }
        [HttpPost]
        public ActionResult SearchDepartmentsByID(string MaBP)
        {


            if (string.IsNullOrEmpty(MaBP))
            {
                return Json(new { success = false, message = "Mã bộ phận không được để trống." });
            }
            var department = _sqlConnectionserver.BoPhans.Where(BP => BP.MaBoPhan == MaBP)
                .Select(BP => new
                {
                    BP.MaBoPhan,
                    BP.TenBP,
                    BP.MaCuaHang,
                    TenCuaHang = BP.CuaHang.TenCH
                })
                .FirstOrDefault();
            if (department == null)
            {
                return Json(new { success = false, message = "Không tìm thấy bộ phận với mã đã cho." });
            }
            return Json(new { success = true, data = department });
        }
        [HttpGet]
        public ActionResult CreateDepartments()
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
        [HttpGet]
        public ActionResult UpdateDepartments()
        {
            var stores = _sqlConnectionserver.CuaHangs
            .Select(ch => new { ch.MaCuaHang, ch.TenCH })
            .ToList();
            ViewBag.Store = new SelectList(stores, "MaCuaHang", "TenCH");
            return View();
        }


        [HttpPost]
        public ActionResult UpdateDepartments(string maBoPhan, string tenBoPhan, int? maCuaHang)
        {
            try
            {
                if (string.IsNullOrEmpty(maBoPhan) || string.IsNullOrEmpty(tenBoPhan) || maCuaHang == null)
                {
                    return Json(new { success = false, message = "Thông tin bộ phận không được để trống." });
                }
                var department = _sqlConnectionserver.BoPhans.FirstOrDefault(bp => bp.MaBoPhan == maBoPhan);

                if (department == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy bộ phận để cập nhật." });
                }
                department.TenBP = tenBoPhan;
                department.MaCuaHang = (int)maCuaHang;
                _sqlConnectionserver.SaveChanges();
                return Json(new { success = true, message = "Cập nhật bộ phận thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }
        [HttpGet]
        public ActionResult DeleteDepartments()
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