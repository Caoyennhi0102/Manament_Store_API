using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manament_Store_API.Controllers
{
    public class IndustryController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionserver;

        public IndustryController()
        {
            _sqlConnectionserver = new SqlConnectionServer();
        }
        // GET: Industry
        public ActionResult Index()
        {

            ViewBag.Languages = _sqlConnectionserver.NgonNgus.ToList();
            return View();
        }
        /*
       protected override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
       {
           if (Session["UserName"] == null)
           {
               actionExecutingContext.Result = RedirectToAction("Login", "Login");
           }
           base.OnActionExecuting(actionExecutingContext);
       }*/
        public ActionResult Dashboard()
        {
            bool IsSessionActive = (Session["UserName"] != null);
            ViewBag.IsSessionActive = IsSessionActive;
            return View();
        }
    }
}