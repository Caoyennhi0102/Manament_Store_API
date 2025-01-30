using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class DeleteStoreController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public DeleteStoreController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: DeleteStore
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteStore(int? maCH)
        {
            try
            {
                if (maCH == null)
                {
                    return Json(new { success = false, message = "Mã cửa hàng không hợp lệ." });
                }
                if (_sqlConnectionserver == null || _sqlConnectionserver.CuaHangs == null)
                {
                    return Json(new { success = false, message = "Không thể truy cập dữ liệu cửa hàng. Vui lòng kiểm tra lại." });
                }
                var store = _sqlConnectionserver.CuaHangs.Find(maCH);
                if (store != null)
                {
                    var deleteStore = _sqlConnectionserver.BoPhans.Where(u => u.MaCuaHang == maCH);


                    if (deleteStore.Any())
                    {

                        _sqlConnectionserver.BoPhans.RemoveRange(deleteStore);
                    }
                    var deleteStoreNV = _sqlConnectionserver.NhanViens.Where(u => u.MaCuaHang == maCH);
                    if (deleteStoreNV.Any())
                    {
                        _sqlConnectionserver.NhanViens.RemoveRange(deleteStoreNV);
                    }

                    _sqlConnectionserver.CuaHangs.Remove(store);
                    _sqlConnectionserver.SaveChanges();


                }
                return Json(new { success = false, message = "Cửa hàng không tồn tại" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình xóa {ex.Message}" });
            }
        }
    }
}