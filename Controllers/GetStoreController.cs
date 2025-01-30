using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class GetStoreController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public GetStoreController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: GetStore
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetStore()
        {
            try
            {
                var getStore = _sqlConnectionserver.CuaHangs.Select(Ch => new { Ch.MaCuaHang, Ch.TenCH }).ToList();
                return Json(getStore, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra trong quá trình gọi danh sách cửa hàng{ex.Message}" });
            }
        }
    }

}