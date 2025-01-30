using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class GetAllUserListController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public GetAllUserListController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: GetAllUserList
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAllUserList()
        {
            try
            {
                var users = _sqlConnectionserver.Users.Select(u => new
                {
                    u.UserID,
                    u.MaNhanVien,
                    u.Roles,
                    UserName = new string('*', u.UserName.Length), // Hiển thị dấu hoa thị theo độ dài UserName
                    PasswordHash = new string('*', u.PasswordHash.Length), // Hiển thị dấu hoa thị cho Password
                    u.TrangThai,
                    u.MaNVQL,
                    u.TrangThaiDuyetQL,

                }).ToList();
                if (users.Count == 0)
                {
                    return Json(new { success = false, message = "Không có người dùng nào." });
                }
                return Json(users);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }
    }
}