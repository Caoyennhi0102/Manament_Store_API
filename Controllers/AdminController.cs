using Manament_Store_API.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Security;
using System.Web.Services.Description;

namespace Manament_Store_API.Controllers
{
    // Đánh dấu phân quyền đăng nhập Admin
    // [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {


        private readonly SqlConnectionServer _sqlConnectionserver;

        public AdminController()
        {
            _sqlConnectionserver = new SqlConnectionServer();

        }
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