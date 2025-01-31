using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class StoreController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public StoreController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetStoreList()
        {
            var stores = _sqlConnectionserver.CuaHangs.ToList();
            return Json(stores, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStore()
        {
            try
            {
                var getStore = _sqlConnectionserver.CuaHangs.Select(Ch => new { Ch.MaCuaHang, Ch.TenCH }).ToList();
                return Json(getStore, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình gọi danh sách cửa hàng{ex.Message}" });
            }
        }
        [HttpPost]
        public JsonResult SearchStore(string maCH)
        {
            try
            {
                if (string.IsNullOrEmpty(maCH) || !int.TryParse(maCH, out var storeId))
                {
                    return Json(new { success = false, message = "Mã cửa hàng không hợp lệ." });
                }


                var store = _sqlConnectionserver.CuaHangs.FirstOrDefault(CH => CH.MaCuaHang == storeId);
                if (store == null)
                {
                    return Json(new { success = false, message = "Cửa hàng không tồn tại" });
                }
                // Trả về thông tin cửa hàng dưới dạng JSON
                return Json(new
                {
                    success = true,
                    store = new
                    {
                        store.MaCuaHang,
                        store.TenCH,
                        store.DiaChi,
                        store.DienThoai,
                        store.Email,
                        store.MST,
                        store.CHTruong,
                        store.SLNV
                    }
                });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình cập nhật. Vui lòng liên hệ với quản trị viên.{ex.Message}" });
            }
        }
        [HttpGet]
        public ActionResult AddStore()
        {
            return View();
        }
        // GET: Admin

        [HttpPost]
        public ActionResult AddStore(string tenCH, string diaChi, string dienThoai, string email, string mst, int chTruong = 123)
        {

            // Kiểm tra tên cửa hàng có để trống hay không 
            if (string.IsNullOrEmpty(tenCH))
            {
                return Json(new { success = false, message = "Tên cửa hàng không được để trống" });

            }
            var IsAddress = new Service.CheckAddresss();
            if (IsAddress.CheckAddress(diaChi))
            {
                return Json(new { success = false, message = "Địa chỉ tạm trú không hợp lệ" });
            }
            var ISemail = new Service.IsEmail();
            if (!ISemail.CheckEmail(email))
            {
                return Json(new { success = false, message = "Địa chỉ email không hợp lệ. Vui lòng kiểm tra lại." });
            }
            var isPhone = new Service.CheckPhone();
            if (!isPhone.IsPhoneNumberValid(dienThoai))
            {
                return Json(new { success = false, message = "Số điện thoại không hợp lệ.Vui lòng kiểm tra lại." });
            }
            var isTaxCode = new Service.IsTaxCode();
            if (!isTaxCode.CheckTaxCode(mst))
            {
                return Json(new { success = false, message = "Mã số thuế không hợp lệ .Vui lòng kiểm tra lại." });
            }
            /*
            var CHTruong = _sqlConnectionserver.NhanViens.FirstOrDefault(nv => nv.MaNhanVien == chTruong);
            if (CHTruong == null)
            {
                return Json(new { success = false, message = "Mã nhân viên trưởng cửa hàng không hợp lệ." });
            }
            */
            var IsStore = new Service.StoreCode();
            int maCH = IsStore.GenerateCuaHangId();
            string formattedIdStore = IsStore.GetFormattedCuaHangId(maCH);
            int soluongNhanVien = _sqlConnectionserver.NhanViens.Count(nv => nv.MaCuaHang == 0);
            /*
            // Truyền dữ liệu qua ViewData
            ViewData["SLNV"] = soluongNhanVien;
            */
            ViewData["SLNV"] = soluongNhanVien;
            var newStore = new CuaHang
            {
                MaCuaHang = maCH,
                TenCH = tenCH,
                DiaChi = diaChi,
                DienThoai = dienThoai,
                Email = email,
                MST = mst,
                CHTruong = 123,
                SLNV = soluongNhanVien
            };
            _sqlConnectionserver.CuaHangs.Add(newStore);
            _sqlConnectionserver.SaveChanges();

            return Json(new { success = true, message = $"Thêm cửa hàng thành công:{formattedIdStore}" });
        }
        [HttpGet]
        public ActionResult UpdateStore()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateStore(int? maCH, string tenCH, string diaChi, string dienThoai, string email, string mst, int? chTruong)
        {
            try
            {
                if (maCH == null)
                {
                    Console.WriteLine("Giá trị maCH: " + maCH);
                    return Json(new { success = false, message = "Mã cửa hàng không hợp lệ." });
                }

                if (_sqlConnectionserver == null || _sqlConnectionserver.CuaHangs == null)
                {
                    return Json(new { success = false, message = "Không thể truy cập dữ liệu cửa hàng. Vui lòng kiểm tra lại." });
                }
                var store = _sqlConnectionserver.CuaHangs.FirstOrDefault(CH => CH.MaCuaHang == maCH);
                if (store == null)
                {
                    return Json(new { success = false, message = "Cửa hàng không tồn tại" });
                }
                if (string.IsNullOrEmpty(tenCH))
                {
                    return Json(new { success = false, message = "Tên cửa hàng không được để trống" });

                }
                var IsAddress = new Service.CheckAddresss();
                if (IsAddress.CheckAddress(diaChi))
                {
                    return Json(new { success = false, message = "Địa chỉ tạm trú không hợp lệ" });
                }
                var isEmail = new Service.IsEmail();
                if (!isEmail.CheckEmail(email))
                {
                    return Json(new { success = false, message = "Địa chỉ email không hợp lệ. Vui lòng kiểm tra lại." });
                }
                var phone = new Service.CheckPhone();
                if (!phone.IsPhoneNumberValid(dienThoai))
                {
                    return Json(new { success = false, message = "Số điện thoại không hợp lệ.Vui lòng kiểm tra lại." });
                }
                var isTaxCode = new Service.IsTaxCode();
                if (!isTaxCode.CheckTaxCode(mst))
                {
                    return Json(new { success = false, message = "Mã số thuế không hợp lệ .Vui lòng kiểm tra lại." });
                }

                if (chTruong == null || !_sqlConnectionserver.NhanViens.Any(nv => nv.MaNhanVien == chTruong.Value))
                {
                    return Json(new { success = false, message = "Mã nhân viên trưởng cửa hàng không hợp lệ." });
                }
                int soluongNhanVien = _sqlConnectionserver.NhanViens.Count(nv => nv.MaCuaHang == maCH);
                // Truyền dữ liệu qua ViewData
                ViewData["SLNV"] = soluongNhanVien;
                store.TenCH = tenCH;
                store.DiaChi = diaChi;
                store.DienThoai = dienThoai;
                store.Email = email;
                store.MST = mst;
                store.CHTruong = (int)chTruong;
                store.SLNV = soluongNhanVien;
                _sqlConnectionserver.SaveChanges();

                return Json(new { success = true, message = "Cập nhật cửa hàng thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình cật nhật{ex.Message}" });
            }


        }
        [HttpGet]
        public ActionResult DeleteStore()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteStore(int? maCH)
        {
            try
            {
                if (maCH == null)
                {
                    return Json(new { success = false, message = "Mã cửa hàng không hợp lệ." });
                }
                if (_sqlConnectionserver == null || _sqlConnectionserver.CuaHangs == null)
                {
                    return Json(new { success = false, message = "Không thể truy cập dữ liệu cửa hàng. Vui lòng kiểm tra lại." });
                }
                var store = _sqlConnectionserver.CuaHangs.Find(maCH);
                if (store != null)
                {
                    var deleteStore = _sqlConnectionserver.BoPhans.Where(u => u.MaCuaHang == maCH);


                    if (deleteStore.Any())
                    {

                        _sqlConnectionserver.BoPhans.RemoveRange(deleteStore);
                    }
                    var deleteStoreNV = _sqlConnectionserver.NhanViens.Where(u => u.MaCuaHang == maCH);
                    if (deleteStoreNV.Any())
                    {
                        _sqlConnectionserver.NhanViens.RemoveRange(deleteStoreNV);
                    }

                    _sqlConnectionserver.CuaHangs.Remove(store);
                    _sqlConnectionserver.SaveChanges();


                }
                return Json(new { success = false, message = "Cửa hàng không tồn tại" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình xóa {ex.Message}" });
            }
        }

    }
}