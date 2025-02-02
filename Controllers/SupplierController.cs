using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Manament_Store_API.Models;
using Manament_Store_API.Service;
using Microsoft.Ajax.Utilities;
namespace Manament_Store_API.Controllers
{
    public class SupplierController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public SupplierController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddSupplier()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddSupplier(string supplierID, string supplierName, string supplierAddress,  string supplierNumberPhone, string supplierEmail, string supplierFax, 
            string supplierWebsite, DateTime supplierDateofEstablishment, string Status, string Note, string taxCode, string contractcode, string accountNumber, string bankName, string Owen)
        
        {
            try
            {
                if (string.IsNullOrEmpty(supplierName))
                {
                    return Json(new { success = false, message = "Vui lòng nhập tên nhà cung cấp" });
                }
                var contract = _sqlConnectionServer.HopDongs.FirstOrDefault(c => c.MaHopDong == contractcode);
                if (contract == null)
                {
                    return Json(new { success = false, message = "Vui lòng nhập mã hợp đồng hợp lệ" });
                }
                if (string.IsNullOrEmpty(supplierAddress))
                {
                    return Json(new { success = false, message = "Vui lòng nhập địa chỉ nhà cung cấp" });
                }
                var isNumberphone = new Service.CheckPhone();
                if (!isNumberphone.IsPhoneNumberValid(supplierNumberPhone))
                {
                    return Json(new { success = false, message = "Vui lòng nhập số điện thoại nhà cung cấp" });
                }
                var isEmail = new Service.IsEmail();
                if (!isEmail.CheckEmail(supplierEmail))
                {
                    return Json(new { success = false, message = "Vui lòng nhập địa chỉ Email nhà cung cấp" });
                }
                var IsFax = new Service.Validator();
                if (!IsFax.CheckFaxNumber(supplierFax))
                {
                    return Json(new { success = false, message = "Vui lòng nhập số Fax của nhà cung cấp" });
                }
                var isTaxCode = new Service.IsTaxCode();
                if (!isTaxCode.CheckTaxCode(taxCode))
                {
                    return Json(new { success = false, message = "Vui lòng nhập mã số thuế phù hợp" });
                }
                var IsWebsite = new Service.WebsiteValidator();
                if (!IsWebsite.CheckWebsite(supplierWebsite))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đúng website của nhà cung cấp" });
                }
                DateTime now = DateTime.Now;
                if (supplierDateofEstablishment > now)
                {
                    return Json(new { success = false, message = "Vui lòng nhập ngày thành lập trước thời gian hiện tại" });
                }
                var acountnumber = _sqlConnectionServer.NhaCungCaps.FirstOrDefault(ac => ac.SoTaiKhoan == accountNumber);
                if(acountnumber != null)
                {
                    return Json(new { success = false, message = "Số tài khoản đã tồn tại" });
                }

                if (string.IsNullOrEmpty(Note))
                {
                    return Json(new { success = false, message = "Vui lòng nhập mô tả về nhà cung cấp" });
                }
                var listStatus = new List<string> { "Còn Hợp Tác", "Ngưng Hợp Tác" };
                if (!listStatus.Contains(Status))
                {
                    return Json(new { success = false, message = "Vui lòng chọn trạng thái hợp lệ" });
                }
                if (string.IsNullOrEmpty(bankName))
                {
                    return Json(new { success = false, message = "Tên ngân hàng không được để trống" });
                }
                if (string.IsNullOrEmpty(Owen))
                {
                    return Json(new { success = false, message = "Tên chủ tài khoản không được để trống" });
                }
                // Lưu nhà cung cấp vào database

                var IsSupplierID = new Service.SupplierCode();
                supplierID = IsSupplierID.GenerateSupplierCode(supplierName);
                var newSupplier = new NhaCungCap
                {
                    MaNhaCungCap = supplierID,
                    MaHopDong = contractcode,
                    TenNhaCungCap = supplierName,
                    DiaChi = supplierAddress,
                    SoDienThoai = supplierNumberPhone,
                    Email = supplierEmail,
                    Fax = supplierFax,
                    MaSoThue = taxCode,
                    Website = supplierWebsite,
                    NgayThanhLap = supplierDateofEstablishment,
                    GhiChu = Note,
                    SoTaiKhoan = accountNumber,
                    ChutaiKhoan = Owen,
                    NganHang = accountNumber,
                    TrangThai = Status,

                };
                _sqlConnectionServer.NhaCungCaps.Add(newSupplier);
                _sqlConnectionServer.SaveChanges();

                
              
                return Json(new { success = true, message = "Thêm nhà cung cấp thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}