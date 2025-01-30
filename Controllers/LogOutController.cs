using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Manament_Store_API.Controllers
{
    public class LogOutController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public LogOutController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: LogOut
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogOut(User user)
        {
            FormsAuthentication.SignOut();
            user.TGDangXuat = DateTime.Now;
            _sqlConnectionServer.SaveChanges();
            Session.Clear();
            return RedirectToAction("Login", "Admin");
        }
    }
}