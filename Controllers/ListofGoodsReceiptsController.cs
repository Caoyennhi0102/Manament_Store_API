using Manament_Store_API.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class ListofGoodsReceiptsController : Controller
    {

        private readonly SqlConnectionServer _sqlConnectionServer;

        public ListofGoodsReceiptsController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }

        // GET: ListofGoodsReceipts
        public ActionResult Index(string searchString, string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.MaPhieuNhapSort = string.IsNullOrEmpty(sortOrder) ? "maPhieuNhap_desc" : "";
            ViewBag.NgayNhapSort = sortOrder == "ngayNhap" ? "ngayNhap_desc" : "ngayNhap";
            ViewBag.TrangThaiSort = sortOrder == "trangThai" ? "trangThai_desc" : "trangThai";

            var EntryForm = _sqlConnectionServer.PhieuNhapHangs.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                EntryForm = EntryForm.Where(p => p.MaPhieuNhap.Contains(searchString) ||
                p.TrangThai.Contains(searchString) || p.NhaCungCap.TenNhaCungCap.Contains(searchString));
            }
            // Sắp xếp
            switch (sortOrder)
            {
                case "maPhieuNhap_desc":
                    EntryForm = EntryForm.OrderByDescending(p => p.MaPhieuNhap);
                    break;
                case "ngayNhap":
                    EntryForm = EntryForm.OrderBy(p => p.NgayNhap);
                    break;
                case "ngayNhap_desc":
                    EntryForm = EntryForm.OrderByDescending(p => p.NgayNhap);
                    break;
                case "trangThai":
                    EntryForm = EntryForm.OrderBy(p => p.TrangThai);
                    break;
                case "trangThai_desc":
                    EntryForm = EntryForm.OrderByDescending(p => p.TrangThai);
                    break;
                default:
                    EntryForm = EntryForm.OrderBy(p => p.MaPhieuNhap);
                    break;
            }
            int pageSize = 10;
            int pageNumBer = (page ?? 1);
            return View(EntryForm.ToPagedList(pageNumBer, pageSize));
        }
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            var entyform = _sqlConnectionServer.PhieuNhapHangs
                .Include("ChiTietPhieuNhaps")
                .Include("NhaCungCap")
                .Include("CuaHang")
                .Include("Kho")
                .FirstOrDefault(p => p.MaPhieuNhap == id);
            if(entyform == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu nhập!";
                return RedirectToAction("Index");
            }
            return View(entyform);
            
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