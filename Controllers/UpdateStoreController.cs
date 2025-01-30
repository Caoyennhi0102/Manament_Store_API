using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class UpdateStoreController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public UpdateStoreController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: UpdateStore
        public ActionResult Index()
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
    }
}