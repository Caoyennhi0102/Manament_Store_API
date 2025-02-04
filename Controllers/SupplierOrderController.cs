using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manament_Store_API.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Services.Discovery;

namespace Manament_Store_API.Controllers
{
    public class SupplierOrderController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;


        public SupplierOrderController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: SupplierOrder
        public ActionResult Index()
        {
            
          
            var oder = _sqlConnectionServer.DonHangNCCs.ToList();
            return View(oder);
        }
        public ActionResult Details(int id)
        {
            var oder = _sqlConnectionServer.DonHangNCCs.Find(id);
            if(oder == null)
            {
                return HttpNotFound();
            }
            return View(oder);
        }
        
        public ActionResult PrintOrder(int id)
        {
            var order = _sqlConnectionServer.DonHangNCCs.Include(d => d.ChiTietDonHangNCCs)
                .FirstOrDefault(d => d.MaDatHang == id);
            if(order == null)
            {
                return HttpNotFound();
            }
            MemoryStream ms = new MemoryStream();
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            document.Add(new Paragraph("Hóa Đơn Đặt Hàng", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
            document.Add(new Paragraph("\nMã Đặt Hàng: " + order.MaDatHang));
            document.Add(new Paragraph("Mã Nhà Cung Cấp: " + order.NhaCungCap.TenNhaCungCap));
            document.Add(new Paragraph("Cửa Hàng Nhận: " + order.CuaHang.TenCH));
            document.Add(new Paragraph("Nhân Viên Đặt: " + order.NhanVien.HoTen));
            document.Add(new Paragraph("Ngày Đặt Hàng: " + order.NgayDatHang.Value.ToString("dd/MM/yyyy")));
            document.Add(new Paragraph("Ngày Giao Hàng: " + order.NgayGHDuKien.ToString("dd/MM/yyyy")));
            document.Add(new Paragraph("Ghi Chú: " + order.GhiChu));
            document.Add(new Paragraph("\nChi Tiết Đơn Đặt Hàng"));

            PdfPTable table = new PdfPTable(12); 
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 40f, 15f, 20f, 25f });

            table.AddCell(new PdfPCell(new Phrase("Mã Chi Tiết Đặt Hàng", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Mã Đặt Hàng", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Mã Sản Phẩm", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Tên Sản Phẩm", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Đơn Vị Tính", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Đơn Giá", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Số Lượng", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
           
            table.AddCell(new PdfPCell(new Phrase("Chiết Khấu", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Ngày Sản Xuất", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Ngày Hết Hạn", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Thành Tiền", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { HorizontalAlignment = Element.ALIGN_CENTER });

            foreach(var item in order.ChiTietDonHangNCCs)
            {
                table.AddCell(new Phrase(item.MaChiTietDonHang));
                table.AddCell(new Phrase(item.MaDatHang));
                table.AddCell(new Phrase(item.MaSanPham));
                table.AddCell(new Phrase(item.TenSanPham));
                table.AddCell(new Phrase(item.DonViTinh));
                table.AddCell(new Phrase(item.DonGia.ToString("N0")));
                table.AddCell(new Phrase(item.SoLuong.ToString()));
                table.AddCell(new Phrase(item.ChietKhau.ToString()));
                table.AddCell(new Phrase(item.NgaySX.ToString("dd/MM/yyyy")));
                table.AddCell(new Phrase(item.NgayHH.ToString("dd/MM/yyyy")));
                table.AddCell(new Phrase((item.SoLuong * item.DonGia).ToString("N0")));

            }

            document.Add(table);
            document.Close();
            writer.Close();

            return File(ms.ToArray(), "application/pdf");
        }
        public ActionResult CreateOrder()
        {
            var liststore = _sqlConnectionServer.CuaHangs.ToList();
            var liststaff = _sqlConnectionServer.NhanViens.ToList();
            var listSupplier = _sqlConnectionServer.NhaCungCaps.ToList();
            ViewBag.MaCuaHang = new SelectList(liststore, "MaCuaHang", "TenCH");
            ViewBag.MaNhanVien = new SelectList(liststaff, "MaNhanVien", "HoTen");
            ViewBag.MaNhaCungCap = new SelectList(listSupplier, "MaNhaCungCap", "TenNhaCungCap");
            var viewModel = new DonHangNCCViewModel
            {
                DonHangNCC = new DonHangNCC(),
                ChiTietDonHangNCCs = new List<ChiTietDonHangNCC>()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(DonHangNCCViewModel viewModelOrder)
        {
            if (ModelState.IsValid)
            {
                _sqlConnectionServer.DonHangNCCs.Add(viewModelOrder.DonHangNCC);
                foreach(var detail in viewModelOrder.ChiTietDonHangNCCs)
                {
                    detail.MaDatHang = viewModelOrder.DonHangNCC.MaDatHang;
                    _sqlConnectionServer.ChiTietDonHangNCCs.Add(detail);
                }
                _sqlConnectionServer.SaveChanges();
                return RedirectToAction("Index");


            }
            var liststore = _sqlConnectionServer.CuaHangs.ToList();
            var liststaff = _sqlConnectionServer.NhanViens.ToList();
            var listSupplier = _sqlConnectionServer.NhaCungCaps.ToList();
            ViewBag.MaCuaHang = new SelectList(liststore, "MaCuaHang", "TenCH", viewModelOrder.DonHangNCC.MaCuaHang);
            ViewBag.MaNhanVien = new SelectList(liststaff, "MaNhanVien", "HoTen", viewModelOrder.DonHangNCC.MaNhanVien);
            ViewBag.MaNhaCungCap = new SelectList(listSupplier, "MaNhaCungCap", "TenNhaCungCap", viewModelOrder.DonHangNCC.MaNhaCungCap);

            return View(viewModelOrder);
        }
        public ActionResult EditOrder(int id)
        {
            var oder = _sqlConnectionServer.DonHangNCCs.Find(id);
            if(oder == null)
            {
                return HttpNotFound();
            }
            var supplierorderdetails = _sqlConnectionServer.ChiTietDonHangNCCs.Where(c => c.MaDatHang == id).ToList();
            var viewModel = new DonHangNCCViewModel
            {
                DonHangNCC = oder,
                ChiTietDonHangNCCs = supplierorderdetails,
            };
            var liststore = _sqlConnectionServer.CuaHangs.ToList();
            var liststaff = _sqlConnectionServer.NhanViens.ToList();
            var listSupplier = _sqlConnectionServer.NhaCungCaps.ToList();
            ViewBag.MaCuaHang = new SelectList(liststore, "MaCuaHang", "TenCH");
            ViewBag.MaNhanVien = new SelectList(liststaff, "MaNhanVien", "HoTen");
            ViewBag.MaNhaCungCap = new SelectList(listSupplier, "MaNhaCungCap", "TenNhaCungCap");
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrder(DonHangNCCViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _sqlConnectionServer.Entry(viewModel.DonHangNCC).State = System.Data.Entity.EntityState.Modified;
                foreach(var detail in viewModel.ChiTietDonHangNCCs)
                {
                    _sqlConnectionServer.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                }
                _sqlConnectionServer.SaveChanges();
                return RedirectToAction("Index");
            }
            var liststore = _sqlConnectionServer.CuaHangs.ToList();
            var liststaff = _sqlConnectionServer.NhanViens.ToList();
            var listSupplier = _sqlConnectionServer.NhaCungCaps.ToList();
            ViewBag.MaCuaHang = new SelectList(liststore, "MaCuaHang", "TenCH", viewModel.DonHangNCC.MaCuaHang);
            ViewBag.MaNhanVien = new SelectList(liststaff, "MaNhanVien", "HoTen", viewModel.DonHangNCC.MaNhanVien);
            ViewBag.MaNhaCungCap = new SelectList(listSupplier, "MaNhaCungCap", "TenNhaCungCap", viewModel.DonHangNCC.MaNhaCungCap);
            return View(viewModel);
        }
        public ActionResult DeleteOrder(int id)
        {
            var oder = _sqlConnectionServer.DonHangNCCs.Find(id);
            if(oder == null)
            {
                return HttpNotFound();
            }
            return View(oder);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var oder = _sqlConnectionServer.DonHangNCCs.Find(id);
            var detail = _sqlConnectionServer.ChiTietDonHangNCCs.Where(c => c.MaDatHang == id).ToList();

            _sqlConnectionServer.ChiTietDonHangNCCs.RemoveRange(detail);
            _sqlConnectionServer.DonHangNCCs.Remove(oder);
            _sqlConnectionServer.SaveChanges();
            return RedirectToAction("Index");

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sqlConnectionServer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}