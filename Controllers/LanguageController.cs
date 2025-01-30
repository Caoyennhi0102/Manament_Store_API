using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manament_Store_API.Models;

namespace Manament_Store_API.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class LanguageController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public LanguageController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        public JsonResult GetLanguages()
        {
            var languages = _sqlConnectionServer.NgonNgus.Select(l => new { l.IDNgonNgu, l.TenNgonNgu })
                .ToList();
            return Json(languages, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeLanguage(int languageId)
        {
            Session["SelectedLanguageId"] = languageId;
            return Json(new { success = true });
            
        }
        // GET: Language
        public ActionResult Index()
        {
            var languages = _sqlConnectionServer.NgonNgus.ToList();
            if (languages == null || !languages.Any())
            {
                return HttpNotFound();
            }
           
            return View(languages);

        }
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            var languages = _sqlConnectionServer.NgonNgus.FirstOrDefault(l => l.IDNgonNgu == id);
            if(languages == null)
            {
                return HttpNotFound();
            }
            return View(languages);
        }
        [HttpGet]
        public ActionResult CreateLanguage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateLanguage(NgonNgu ngonNgu)
        {
            if (ModelState.IsValid)
            {
                _sqlConnectionServer.NgonNgus.Add(ngonNgu);
                _sqlConnectionServer.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ngonNgu);
        }
        public ActionResult EditLanguage( int id)
        {
            var language = _sqlConnectionServer.NgonNgus.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }
        [HttpPost]
        public ActionResult EditLanguage(int id, NgonNgu ngonngu)
        {
            if (ModelState.IsValid)
            {
                var existingLanguage = _sqlConnectionServer.NgonNgus.Find(id);
                if (existingLanguage != null)
                {
                    _sqlConnectionServer.Entry(ngonngu).State = EntityState.Modified;
                    _sqlConnectionServer.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return View(ngonngu);
        }
        public ActionResult DeleteLanguage(int id)
        {
            var language = _sqlConnectionServer.NgonNgus.Find(id);
            if(language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }
        [HttpPost, ActionName("DeleteLanguage")]
        public ActionResult DeleteConfirmed(int id)
        {
            var language = _sqlConnectionServer.NgonNgus.Find(id);
            if (language == null)
            {
                return HttpNotFound(); // Trả về lỗi 404 nếu không tìm thấy
            }
            _sqlConnectionServer.NgonNgus.Remove(language);
            _sqlConnectionServer.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}