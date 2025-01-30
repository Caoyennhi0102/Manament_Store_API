using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class SearchUserByMaNVController : Controller
    {

        public readonly SqlConnectionServer _sqlConnectionserver;

        public SearchUserByMaNVController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: SearchUserByMaNV
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult SearchUserByMaNV(int MaNV)
        {
            var user = _sqlConnectionserver.Users.FirstOrDefault(u => u.MaNhanVien == MaNV);
            if (user != null)
            {
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        UserID = user.UserID,
                        MaNhanVien = user.MaNhanVien,
                        Roles = user.Roles,
                        TrangThai = user.TrangThai,
                        TrangThaiDuyetQL = user.TrangThaiDuyetQL
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Không tìm thấy User." }, JsonRequestBehavior.AllowGet);
        }
    }
}