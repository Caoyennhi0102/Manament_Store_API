using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manament_Store_API.Models;

namespace Manament_Store_API.Controllers
{
    
    public class OrderStatisticsController : Controller
    {
        public readonly SqlConnectionServer _sqlConnectionServer;

        public OrderStatisticsController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: OrderStatistics
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetOrder()
        {
            var donHangTheoThang = _sqlConnectionServer.DonHangNCCs
               .Where(d => d.NgayDatHang.HasValue)
               .GroupBy(d => new { d.NgayDatHang.Value.Year, d.NgayDatHang.Value.Month })
               .Select(g => new { Thang = g.Key.Month, Nam = g.Key.Year, SoLuong = g.Count() })
               .OrderBy(g => g.Nam).ThenBy(g => g.Thang)
               .ToList();

            var tongTienTheoNCC = _sqlConnectionServer.DonHangNCCs
              .GroupBy(d => d.MaNhaCungCap)
              .Select(g => new { MaNCC = g.Key, TongTien = g.Sum(d => d.TongTien) })
              .ToList();

            var donHangTheoTrangThai = _sqlConnectionServer.DonHangNCCs
             .GroupBy(d => d.TrangThai)
             .Select(g => new { TrangThai = g.Key, SoLuong = g.Count() })
             .ToList();

            var tongTienTheoCuaHang = _sqlConnectionServer.DonHangNCCs
              .GroupBy(d => d.MaCuaHang)
              .Select(g => new { MaCuaHang = g.Key, TongTien = g.Sum(d => d.TongTien) })
              .ToList();

            var data = new
            {
                DonHangTheoThang = donHangTheoThang,
                TongTienTheoNCC = tongTienTheoNCC,
                DonHangTheoTrangThai = donHangTheoTrangThai,
                TongTienTheoCuaHang = tongTienTheoCuaHang
            };
            return Json(data, JsonRequestBehavior.AllowGet);

        }
    }
}