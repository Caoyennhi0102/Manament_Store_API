using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manament_Store_API.Models;

namespace Manament_Store_API.Controllers
{
    public class ImportGoodsIntoWarehouseController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public ImportGoodsIntoWarehouseController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: ImportGoodsIntoWarehouse
        public ActionResult Index()
        {
            var supplier = _sqlConnectionServer.NhaCungCaps.ToList();
            ViewBag.DSsupplier = new SelectList(supplier, "MaNhaCungCap", "TenNhaCungCap");
            var store = _sqlConnectionServer.CuaHangs.ToList();
            ViewBag.DSStore = new SelectList(store, "MaCuahang", "TenCH");
            var warehouse = _sqlConnectionServer.Khoes.ToList();
            ViewBag.Warehouse = new SelectList(warehouse, "MaKho", "TenKho");
            var category = _sqlConnectionServer.DanhMucSanPhams.ToList();
            ViewBag.Category = new SelectList(category, "MaDanhMucSP", "TenDanhMuc");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhieuNhapHang phieuNhap)
        {
            try
            {
                if (Session["MaNhanVien"] != null)
                {
                    phieuNhap.MaNhanVien = Convert.ToInt32(Session["MaNhanVien"]);
                }
                else
                {
                    throw new Exception("Chưa xác định mã nhân viên.");
                }
                if (string.IsNullOrEmpty(phieuNhap.MaPhieuNhap))
                {
                    var importCode = new Service.ImportCode();
                    string newCode;
                    do
                    {
                        newCode = importCode.GenerateImportCode();
                    } while (_sqlConnectionServer.PhieuNhapHangs.Any(p => p.MaPhieuNhap == newCode));
                    phieuNhap.MaPhieuNhap = newCode;
                }
                phieuNhap.NgayNhap = DateTime.Now;
                phieuNhap.TrangThai = "Đã nhập";

                _sqlConnectionServer.PhieuNhapHangs.Add(phieuNhap);

                if(phieuNhap.ChiTietPhieuNhaps != null)
                {
                    foreach(var ct in phieuNhap.ChiTietPhieuNhaps)
                    {
                        ct.MaPhieuNhap = phieuNhap.MaPhieuNhap;

                        ct.ThanhTien = ct.SoLuong * ct.DonGia;

                        var goods = _sqlConnectionServer.HangHoas.FirstOrDefault(h => h.MaSanPhamNCC == ct.MaSanPham);
                        var IsCodeProductCode = new Service.ProductCode();
                        var IsProductCode = new Service.CodeProduct();
                        string maHH = IsProductCode.GenerateMaHangHoa();
                        if (goods == null)
                        {
                            goods = new HangHoa
                            {

                                MaHangHoa = IsCodeProductCode.GenerateBarcodeWithZXing(maHH),
                                TenHangHoa = ct.TenSanPham,
                                MaDanhMucSP = phieuNhap.MaDanhMuc,
                                MaNhaCungCap = phieuNhap.MaNhaCungCap,
                                DonGia = ct.DonGia,
                                DonViTinh = ct.DonViTinh,
                                SoLuong = ct.SoLuong,
                                NgayNhap = phieuNhap.NgayNhap,
                                NgaySX = ct.NgaySanXuat,
                                NgayHetHan = ct.NgayHetHan,
                                GhiChu = phieuNhap.GhiChu,
                                Thue = 0,
                                TrangThai = "Mới nhập"

                            };
                            _sqlConnectionServer.HangHoas.Add(goods);
                        }
                        else
                        {
                            goods.SoLuong += ct.SoLuong;
                        }
                        var warehouseRedord = _sqlConnectionServer.Khoes.FirstOrDefault(
                            k => k.MaHangHoa == goods.MaHangHoa && k.MaCuaHang == phieuNhap.MaCuaHang && k.MaKho == phieuNhap.MaKho);
                        if (warehouseRedord == null)
                        {
                            warehouseRedord = new Kho
                            {
                                MaCuaHang = phieuNhap.MaCuaHang,
                                MaKho = phieuNhap.MaKho,
                                MaHangHoa = goods.MaHangHoa,
                                SoLuongTon = ct.SoLuong,
                                TenKho = _sqlConnectionServer.CuaHangs
                                .FirstOrDefault(c => c.MaCuaHang == phieuNhap.MaCuaHang)?.TenCH
                                
                            };
                            _sqlConnectionServer.Khoes.Add(warehouseRedord);
                        }
                        else
                        {
                            warehouseRedord.SoLuongTon += ct.SoLuong;
                        }
                    }
                    
                }
                _sqlConnectionServer.SaveChanges();
                TempData["SuccessMessage"] = "Nhập hàng vào kho thành công!";
                return RedirectToAction("Index");

            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi nhập hàng: " + ex.Message;
                // Load lại dữ liệu dropdown list
                var supplier = _sqlConnectionServer.NhaCungCaps.ToList();
                ViewBag.DSsupplier = new SelectList(supplier, "MaNhaCungCap", "TenNhaCungCap");
                var store = _sqlConnectionServer.CuaHangs.ToList();
                ViewBag.DSStore = new SelectList(store, "MaCuahang", "TenCH");
                var warehouse = _sqlConnectionServer.Khoes.ToList();
                ViewBag.Warehouse = new SelectList(warehouse, "MaKho", "TenKho");
                var category = _sqlConnectionServer.DanhMucSanPhams.ToList();
                ViewBag.Category = new SelectList(category, "MaDanhMuc", "TenDanhMuc");
                return View(phieuNhap);
            }
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