using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manament_Store_API.Models;
namespace Manament_Store_API.Controllers
{
    public class TranslationController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public TranslationController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: Translation
        public ActionResult Index()
        {
            var translations = _sqlConnectionServer.BanDiches.Include("NgonNgu").ToList();
            return View(translations);
        }
        [HttpGet]
        public ActionResult CreateTranslation()
        {
            ViewBag.NgonNgus = new SelectList(_sqlConnectionServer.NgonNgus, "IDNgonNgu", "TenNgonNgu");
            return View();
        }
        [HttpPost]
        public ActionResult CreateTranslation(BanDich banDich)
        {
            if (ModelState.IsValid)
            {
                _sqlConnectionServer.BanDiches.Add(banDich);
                _sqlConnectionServer.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NgonNgus = new SelectList(_sqlConnectionServer.NgonNgus, "IDNgonNgu", "TenNgonNgu");
            return View(banDich);
        }
        [HttpGet]
        public ActionResult EditTranslation(int id)
        {
            var banDich = _sqlConnectionServer.BanDiches.Find(id);
            if (banDich == null)
            {
                return HttpNotFound();
            }
            ViewBag.NgonNgus = new SelectList(_sqlConnectionServer.NgonNgus, "IDNgonNgu", "TenNgonNgu");
            return View(banDich);
        }
        [HttpPost]
        public ActionResult EditTranslation(int id,BanDich banDich)
        {
            if (ModelState.IsValid)
            {
                var IsbanDich = _sqlConnectionServer.BanDiches.Find(id);
                if (IsbanDich != null)
                {
                    _sqlConnectionServer.Entry(banDich).State = System.Data.Entity.EntityState.Modified;
                    _sqlConnectionServer.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            ViewBag.NgonNgus = new SelectList(_sqlConnectionServer.NgonNgus, "IDNgonNgu", "TenNgonNgu", banDich.IDNgonNgu);
            return View(banDich);
        }
        [HttpGet]
        public ActionResult DeleteTranslation(int id)
        {
            var banDich = _sqlConnectionServer.BanDiches.Find(id);
            if(banDich == null)
            {
              return HttpNotFound();
            }
            return View(banDich);
        }
        [HttpPost, ActionName("DeleteTranslation")]
        public ActionResult DeleteConfirmed(int id)
        {
            var banDich = _sqlConnectionServer.BanDiches.Find(id);
            if(banDich != null)
            {
                _sqlConnectionServer.BanDiches.Remove(banDich);
                _sqlConnectionServer.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}