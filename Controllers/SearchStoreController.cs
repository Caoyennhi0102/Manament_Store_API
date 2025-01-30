using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class SearchStoreController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public SearchStoreController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: SearchStore
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SearchStore(string maCH)
        {
            try
            {
                if (string.IsNullOrEmpty(maCH) || !int.TryParse(maCH, out var storeId))
                {
                    return Json(new { success = false, message = "Mã cửa hàng không hợp lệ." });
                }


                var store = _sqlConnectionserver.CuaHangs.FirstOrDefault(CH => CH.MaCuaHang == storeId);
                if (store == null)
                {
                    return Json(new { success = false, message = "Cửa hàng không tồn tại" });
                }
                // Trả về thông tin cửa hàng dưới dạng JSON
                return Json(new
                {
                    success = true,
                    store = new
                    {
                        store.MaCuaHang,
                        store.TenCH,
                        store.DiaChi,
                        store.DienThoai,
                        store.Email,
                        store.MST,
                        store.CHTruong,
                        store.SLNV
                    }
                });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình cập nhật. Vui lòng liên hệ với quản trị viên.{ex.Message}" });
            }
        }
    }
}