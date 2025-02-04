using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Manament_Store_API.Models;
using Manament_Store_API.Service;
using Microsoft.Ajax.Utilities;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Properties;



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
        [HttpPost]
        public JsonResult SearchSupplierID(string supplierID)
        {
            try
            {
                if (string.IsNullOrEmpty(supplierID))
                {
                    return Json(new { success = false, message = "Vui lòng nhập mã nhà cung cấp để tìm kiếm " });
                }
                var supplier = _sqlConnectionServer.NhaCungCaps.FirstOrDefault(n => n.MaNhaCungCap == supplierID);
                if(supplier == null)
                {
                    return Json(new { success = false, message = "Mã nhà cung cấp không tồn tại" });
                }
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        supplier.TenNhaCungCap,
                        supplier.MaHopDong,
                        supplier.DiaChi,
                        supplier.SoDienThoai,
                        supplier.Email,
                        supplier.MaSoThue,
                        supplier.Fax,
                        supplier.Website,
                        supplier.NgayThanhLap,
                        supplier.NganHang,
                        supplier.SoTaiKhoan,
                        supplier.ChutaiKhoan,
                        supplier.TrangThai
                    }
                },JsonRequestBehavior.AllowGet);
                

            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi:{ex.Message}" });
            }
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
        [HttpGet]
        public ActionResult UpdateSupplier()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateSupplier(string supplierID, NhaCungCap Updatesupplier, string contractcode)
        {
            try
            {
                if (string.IsNullOrEmpty(supplierID))
                {
                    return Json(new { success = false, message = "Vui lòng nhập mã nhà cung cấp để tìm kiếm " });
                }
                var supplier = _sqlConnectionServer.NhaCungCaps.FirstOrDefault(s => s.MaNhaCungCap == supplierID);
                if(supplier == null)
                {
                    return Json(new { success = false, message = "Mã nhà cung cấp không tồn tại" });
                }
                if (string.IsNullOrEmpty(Updatesupplier.TenNhaCungCap))
                {
                    return Json(new { success = false, message = "Vui lòng nhập tên nhà cung cấp" });
                }
                var contract = _sqlConnectionServer.HopDongs.FirstOrDefault(c => c.MaHopDong == contractcode);
                if (contract == null)
                {
                    return Json(new { success = false, message = "Vui lòng nhập mã hợp đồng hợp lệ" });
                }
                if (string.IsNullOrEmpty(Updatesupplier.DiaChi))
                {
                    return Json(new { success = false, message = "Vui lòng nhập địa chỉ nhà cung cấp" });
                }
                var isNumberphone = new Service.CheckPhone();
                if (!isNumberphone.IsPhoneNumberValid(Updatesupplier.SoDienThoai))
                {
                    return Json(new { success = false, message = "Vui lòng nhập số điện thoại nhà cung cấp" });
                }
                var isEmail = new Service.IsEmail();
                if (!isEmail.CheckEmail(Updatesupplier.Email))
                {
                    return Json(new { success = false, message = "Vui lòng nhập địa chỉ Email nhà cung cấp" });
                }
                var IsFax = new Service.Validator();
                if (!IsFax.CheckFaxNumber(Updatesupplier.Fax))
                {
                    return Json(new { success = false, message = "Vui lòng nhập số Fax của nhà cung cấp" });
                }
                var isTaxCode = new Service.IsTaxCode();
                if (!isTaxCode.CheckTaxCode(Updatesupplier.MaSoThue))
                {
                    return Json(new { success = false, message = "Vui lòng nhập mã số thuế phù hợp" });
                }
                var IsWebsite = new Service.WebsiteValidator();
                if (!IsWebsite.CheckWebsite(Updatesupplier.Website))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đúng website của nhà cung cấp" });
                }
                DateTime now = DateTime.Now;
                if (Updatesupplier.NgayThanhLap > now)
                {
                    return Json(new { success = false, message = "Vui lòng nhập ngày thành lập trước thời gian hiện tại" });
                }
                var oldSupplier = _sqlConnectionServer.NhaCungCaps.Find(Updatesupplier.SoTaiKhoan);
                if (oldSupplier != null && oldSupplier.SoTaiKhoan != Updatesupplier.SoTaiKhoan)
                {
                    var accountNumberExists = _sqlConnectionServer.NhaCungCaps.Any(ac => ac.SoTaiKhoan == Updatesupplier.SoTaiKhoan);
                    if (accountNumberExists)
                    {
                        return Json(new { success = false, message = "Số tài khoản đã tồn tại" });
                    }
                }

                if (string.IsNullOrEmpty(Updatesupplier.GhiChu))
                {
                    return Json(new { success = false, message = "Vui lòng nhập lý do cập nhật" });
                }
                var listStatus = new List<string> { "Còn Hợp Tác", "Ngưng Hợp Tác" };
                if (!listStatus.Contains(Updatesupplier.TrangThai))
                {
                    return Json(new { success = false, message = "Vui lòng chọn trạng thái hợp lệ" });
                }
                if (string.IsNullOrEmpty(Updatesupplier.NganHang))
                {
                    return Json(new { success = false, message = "Tên ngân hàng không được để trống" });
                }
                if (string.IsNullOrEmpty(Updatesupplier.ChutaiKhoan))
                {
                    return Json(new { success = false, message = "Tên chủ tài khoản không được để trống" });
                }

                supplier.TenNhaCungCap = Updatesupplier.TenNhaCungCap;
                supplier.MaHopDong = Updatesupplier.MaHopDong;
                supplier.DiaChi = Updatesupplier.DiaChi;
                supplier.SoDienThoai = Updatesupplier.SoDienThoai;
                supplier.Email = Updatesupplier.Email;
                supplier.MaSoThue = Updatesupplier.MaSoThue;
                supplier.Fax = Updatesupplier.Fax;
                supplier.Website = Updatesupplier.Website;
                supplier.NgayThanhLap = Updatesupplier.NgayThanhLap;
                supplier.NganHang = Updatesupplier.NganHang;
                supplier.ChutaiKhoan = Updatesupplier.ChutaiKhoan;
                supplier.SoTaiKhoan = Updatesupplier.SoTaiKhoan;
                supplier.TrangThai = Updatesupplier.TrangThai;
                supplier.GhiChu = Updatesupplier.GhiChu;
                _sqlConnectionServer.SaveChanges();
                return Json(new { success = true, message = "Cập nhật thông tin nhà cung cấp thành công" });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi:{ex.Message}" });
            }
        }
        [HttpGet]
        public ActionResult DeleteSupplier()
        {
            return View();

        }
        [HttpPost]
        public ActionResult DeleteSupplier(string supplierId)
        {
            try
            {

                if (string.IsNullOrEmpty(supplierId))
                {
                    return Json(new { success = false, message = "Vui lòng nhập mã nhà cung cấp" });
                }
                var supplier = _sqlConnectionServer.NhaCungCaps.Find(supplierId);
                if(supplier != null)
                {
                    var deletesupplierIdHH = _sqlConnectionServer.HangHoas.Where(s => s.MaNhaCungCap == supplierId);
                    if (deletesupplierIdHH.Any())
                    {
                        _sqlConnectionServer.HangHoas.RemoveRange(deletesupplierIdHH);

                    }
                    var deletesupplierIdPN = _sqlConnectionServer.PhieuNhapHangs.Where(s => s.MaNhaCungCap == supplierId);
                    if (deletesupplierIdPN.Any())
                    {
                        _sqlConnectionServer.PhieuNhapHangs.RemoveRange(deletesupplierIdPN);
                    }
                    var deletesupplierIdTr = _sqlConnectionServer.PhieuTraHangs.Where(s => s.MaNhaCungCap == supplierId);
                    if (deletesupplierIdTr.Any())
                    {
                        _sqlConnectionServer.PhieuTraHangs.RemoveRange(deletesupplierIdTr);
                    }

                    _sqlConnectionServer.NhaCungCaps.Remove(supplier);

                    return Json(new { success = true, message = "Xóa nhà cung cấp thành công" });
                }
                return Json(new { success = false, message = "Xóa nhà cung cấp không thành công" });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi:{ex.Message}" });
            }
        }
        [HttpGet]
        public ActionResult SupplierList()
        {
            var supplier = _sqlConnectionServer.NhaCungCaps.ToList();
            if(supplier == null || supplier.Count == 0)
            {
                // Nếu danh sách rỗng, trả về thông báo
                TempData["ErrorMessage"] = "Không có dữ liệu nhà cung cấp để in.";
                return View();
            }
            return View(supplier);
        }
        public ActionResult PrintSupplierList()
        {
            var supplier = _sqlConnectionServer.NhaCungCaps.ToList();
            if (supplier == null || supplier.Count == 0)
            {
                // Nếu danh sách rỗng, trả về thông báo
                TempData["ErrorMessage"] = "Không có dữ liệu nhà cung cấp để in.";
                return RedirectToAction("SupplierList");
            }
            
            byte[] pdfBytes = GeneratePdf(supplier);

            return File(pdfBytes, "application/pdf", "DanhSachNhaCungCap.pdf");
        }

        private byte[] GeneratePdf(List<NhaCungCap> supplier)
        {
            using(MemoryStream ms = new MemoryStream())
            {
                using(PdfWriter writer = new PdfWriter(ms))
                {
                    using(PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf);
                        document.Add(new Paragraph("DANH SÁCH NHÀ CUNG CẤP")
                   .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                   .SetFontSize(16)
                   .SetTextAlignment(TextAlignment.CENTER));

                        if (supplier.Count > 0)
                        {
                            Table table = new Table(15);
                            table.AddHeaderCell("Mã Nhà Cung Cấp");
                            table.AddHeaderCell("Tên Nhà Cung Cấp");
                            table.AddHeaderCell("Mã Hợp Đồng");
                            table.AddHeaderCell("Mã Số Thuế");
                            table.AddHeaderCell("Địa Chỉ");
                            table.AddHeaderCell("Điện Thoại");
                            table.AddHeaderCell("Email");
                            table.AddHeaderCell("Website");
                            table.AddHeaderCell("Fax");
                            table.AddHeaderCell("Ngày Thành Lập");
                            table.AddHeaderCell("Ngân Hàng Thụ Hưởng");
                            table.AddHeaderCell("Số Tài Khoản Ngân Hàng");
                            table.AddHeaderCell("Chủ Tài Khoản");
                            table.AddHeaderCell("Ghi Chú");
                            table.AddHeaderCell("Trạng Thái");

                            foreach (var suppliers in supplier)
                            {
                                table.AddCell(suppliers.MaNhaCungCap);
                                table.AddCell(suppliers.TenNhaCungCap);
                                table.AddCell(suppliers.MaHopDong);
                                table.AddCell(suppliers.MaSoThue);
                                table.AddCell(suppliers.DiaChi);
                                table.AddCell(suppliers.SoDienThoai);
                                table.AddCell(suppliers.Email);
                                table.AddCell(suppliers.Website);
                                table.AddCell(suppliers.Fax);
                                table.AddCell(suppliers.NgayThanhLap.ToString());
                                table.AddCell(suppliers.NganHang);
                                table.AddCell(suppliers.SoTaiKhoan);
                                table.AddCell(suppliers.ChutaiKhoan);
                                table.AddCell(suppliers.GhiChu);
                                table.AddCell(suppliers.TrangThai);
                            }
                            document.Add(table);
                        }
                        else
                        {
                            document.Add(new Paragraph("Không có dữ liệu nhà cung cấp.")); 
                        }
                        document.Close();
                    }
                }
                return ms.ToArray();
            }
        }
    }
}