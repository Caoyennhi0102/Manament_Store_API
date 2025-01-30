using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class AddStoreController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public AddStoreController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: AddStore
        [HttpGet]
        public ActionResult Index()
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
    }
}