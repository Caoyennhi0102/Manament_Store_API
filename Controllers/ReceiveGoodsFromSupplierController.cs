using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class ReceiveGoodsFromSupplierController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public ReceiveGoodsFromSupplierController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }

        // GET: ReceiveGoodsFromSupplier
        public ActionResult Index()
        {
            try
            {
                var pendingOrders = _sqlConnectionServer.DonHangNCCs
                    .Include(d => d.ChiTietDonHangNCCs)
                    .Include(d => d.NhaCungCap)
                    .Where(d => d.TrangThai == "Chờ nhận hàng")
                    .OrderBy(d => d.NgayDatHang)
                    .ToList();

                return View(pendingOrders);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải dữ liệu: {ex.Message}";
                return View(new List<DonHangNCC>());
            }
        }
        public ActionResult Details(int id)
        {
            try
            {
                var order = _sqlConnectionServer.DonHangNCCs
                    .Include(d => d.ChiTietDonHangNCCs)
                    .Include(d => d.NhaCungCap)
                    .FirstOrDefault(d => d.MaDatHang == id);

                if (order == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đơn hàng!";
                    return RedirectToAction("Index");
                }

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải chi tiết đơn hàng: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmReceipt(int id)
        {
            try
            {
                using (var transaction = _sqlConnectionServer.Database.BeginTransaction())
                {
                    var order = _sqlConnectionServer.DonHangNCCs
                        .Include(d => d.ChiTietDonHangNCCs)
                        .FirstOrDefault(d => d.MaDatHang == id);

                    if (order == null)
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy đơn hàng!";
                        return RedirectToAction("Index");
                    }

                    if (order.TrangThai != "Chờ nhận hàng")
                    {
                        TempData["ErrorMessage"] = "Đơn hàng không ở trạng thái chờ nhận!";
                        return RedirectToAction("Index");
                    }
                    // Chỉ cập nhật trạng thái đơn hàng, không cập nhật tồn kho
                    order.TrangThai = "Đã nhận hàng";
                    order.NgayNHTT = DateTime.Now;

                    _sqlConnectionServer.SaveChanges();
                    transaction.Commit();

                    TempData["SuccessMessage"] = "Xác nhận nhận hàng thành công!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xác nhận nhận hàng: {ex.Message}";
            }

            return RedirectToAction("Index");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePO(int id, HttpPostedFileBase filePO)
        {
            try
            {
                var order = _sqlConnectionServer.DonHangNCCs.FirstOrDefault(d => d.MaDatHang == id);
                if (order == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đơn hàng!";
                    return RedirectToAction("Index");
                }
                if(filePO != null && filePO.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(filePO.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Uploads/PO/"), fileName);
                    filePO.SaveAs(filePath);

                    order.POFilePath = "/Uploads/PO/" + fileName;
                    _sqlConnectionServer.SaveChanges();
                    TempData["SuccessMessage"] = "Lưu PO thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Vui lòng chọn một hình ảnh!";
                }
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi lưu PO: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

    }
}