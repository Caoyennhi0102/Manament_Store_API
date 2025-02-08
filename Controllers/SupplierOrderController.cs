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
        public ActionResult Index(string filter = "all")
        {
            IQueryable<DonHangNCC> query = _sqlConnectionServer.DonHangNCCs
            .Include(o => o.NhaCungCap)
             .Include(o => o.CuaHang)
             .Include(o => o.NhanVien);
            switch (filter.ToLower())
            {
                case "delivered":
                    query = query.Where(o => o.TrangThai == "Đã giao");
                    ViewBag.FilterMessage = "Đã giao";
                    break;
                case "notdelivered":
                    query = query.Where(o => o.TrangThai == "Chưa giao");
                    ViewBag.FilterMessage = "Chưa giao";
                    break;
                case "late":
                    query = query.Where(o => o.TrangThai == "Đã giao"
                             && o.NgayNHTT != null
                             && o.NgayNHTT > o.NgayGHDuKien);
                    ViewBag.FilterMessage = "Giao trễ";
                    break;
                default:
                    ViewBag.FilterMessage = "Tất cả";
                    break;
            }
            var orders = query.ToList();
            if (!orders.Any())
            {
                ViewBag.Message = $"Không có đơn hàng {ViewBag.FilterMessage.ToLower()} nào";
            }
            ViewBag.CurrentFilter = filter;
            return View(orders);

        }
        public ActionResult Details(int id)
        {
            var oder = _sqlConnectionServer.DonHangNCCs.Find(id);
            if (oder == null)
            {
                return HttpNotFound();
            }
            return View(oder);
        }

        public ActionResult PrintOrder(int id)
        {
            var order = _sqlConnectionServer.DonHangNCCs.Include(d => d.ChiTietDonHangNCCs)
                .FirstOrDefault(d => d.MaDatHang == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            MemoryStream ms = new MemoryStream();
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();

            Paragraph nationalTitle = new Paragraph("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            nationalTitle.Alignment = Element.ALIGN_CENTER;
            document.Add(nationalTitle);

            Paragraph slogan = new Paragraph("Độc lập - Tự do - Hạnh phúc", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
            slogan.Alignment = Element.ALIGN_CENTER;
            document.Add(slogan);


            document.Add(new Paragraph("\n"));


            document.Add(new Paragraph("Hóa Đơn Đặt Hàng", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)));
            document.Add(new Paragraph("\nMã Đặt Hàng: " + order.MaDatHang));
            document.Add(new Paragraph("Mã Nhà Cung Cấp: " + order.NhaCungCap.TenNhaCungCap));
            document.Add(new Paragraph("Cửa Hàng Nhận: " + order.CuaHang.TenCH));
            document.Add(new Paragraph("Nhân Viên Đặt: " + order.NhanVien.HoTen));
            document.Add(new Paragraph("Ngày Đặt Hàng: " + order.NgayDatHang.Value.ToString("dd/MM/yyyy")));
            document.Add(new Paragraph("Ngày Nhận Hàng Thực Tế: " + order.NgayGHDuKien.ToString("dd/MM/yyyy")));
            document.Add(new Paragraph("Ngày Giao Hàng: " + order.NgayGHDuKien.ToString("dd/MM/yyyy")));
            document.Add(new Paragraph("Ghi Chú: " + order.GhiChu));
            document.Add(new Paragraph("\nChi Tiết Đơn Đặt Hàng"));


            PdfPTable table = new PdfPTable(13);
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
            document.Add(new Paragraph("\n\n\n"));
            // Tạo bảng 3 cột cho phần ký tên
            PdfPTable signatureTable = new PdfPTable(3);
            signatureTable.WidthPercentage = 100;
            signatureTable.SetWidths(new float[] { 1f, 1f, 1f });

            // Ô ký Nhân viên thu mua (trái)
            PdfPCell leftCell = new PdfPCell();
            leftCell.Border = PdfPCell.NO_BORDER;
            leftCell.AddElement(new Paragraph("NHÂN VIÊN THU MUA", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
            leftCell.AddElement(new Paragraph("(Ký và ghi rõ họ tên)"));
            leftCell.AddElement(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1))));
            signatureTable.AddCell(leftCell);

            // Ô ký Quản lý (giữa)
            PdfPCell middleCell = new PdfPCell();
            middleCell.Border = PdfPCell.NO_BORDER;
            middleCell.AddElement(new Paragraph("QUẢN LÝ DUYỆT", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
            middleCell.AddElement(new Paragraph("(Ký và ghi rõ họ tên)"));
            middleCell.AddElement(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, 1))));
            signatureTable.AddCell(middleCell);

            // Ô ký Giám đốc (phải)
            PdfPCell rightCell = new PdfPCell();
            rightCell.Border = PdfPCell.NO_BORDER;
            rightCell.AddElement(new Paragraph("GIÁM ĐỐC XÁC NHẬN", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
            rightCell.AddElement(new Paragraph("(Ký và ghi rõ họ tên)"));
            rightCell.AddElement(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100f, BaseColor.BLACK, Element.ALIGN_RIGHT, 1))));
            signatureTable.AddCell(rightCell);

            document.Add(signatureTable);



            foreach (var item in order.ChiTietDonHangNCCs)
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
                foreach (var detail in viewModelOrder.ChiTietDonHangNCCs)
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
            if (oder == null)
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
                foreach (var detail in viewModel.ChiTietDonHangNCCs)
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
            if (oder == null)
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