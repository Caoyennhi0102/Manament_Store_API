using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manament_Store_API.Models;
namespace Manament_Store_API.Controllers
{
    public class SupplierOrderApprovalController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public SupplierOrderApprovalController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: SupplierOrderApproval
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PendingOrders()
        {
            var pendingOrders = _sqlConnectionServer.DonHangNCCs.Include
                (o => o.NhaCungCap)
                .Where(o => o.TrangThai == "Chờ duyệt").ToList();
            return View(pendingOrders);
        }
        public ActionResult ReviewOrder(int id)
        {
            var order = _sqlConnectionServer.DonHangNCCs.Include(o => o.NhaCungCap)
                .Include(o => o.ChiTietDonHangNCCs).FirstOrDefault(o => o.MaDatHang == id);

            if(order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveOrder(int id)
        {
            var order = _sqlConnectionServer.DonHangNCCs.Find(id);
            if(order == null)
            {
                return HttpNotFound();
            }
            order.TrangThai = "Đã duyệt";
            _sqlConnectionServer.SaveChanges();
            return RedirectToAction("PendingOrders");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectOrder(int id)
        {
            var order = _sqlConnectionServer.DonHangNCCs.Find(id);
            if(order == null)
            {
                return HttpNotFound();
            }
            order.TrangThai = "Từ chối";
            _sqlConnectionServer.SaveChanges();
            return RedirectToAction("PendingOrders");
        }
    }
}