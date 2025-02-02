using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manament_Store_API.Models;
namespace Manament_Store_API.Controllers
{
    public class ContractController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public ContractController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: Contract
        public ActionResult ContractCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContractCreate(HopDong contract, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if(file != null && file.ContentLength > 0)
                {
                    try
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Uploads/Contrat"), fileName);
                        file.SaveAs(path);

                        contract.DuongDan = "/Uploads/Contrat/" + fileName;

                    }
                    catch(Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi lưu file: " + ex.Message);
                        return View(contract);
                    }
                }
                contract.NgayLuu = DateTime.Now;
                _sqlConnectionServer.HopDongs.Add(contract);
                _sqlConnectionServer.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contract);
        }
        public ActionResult ContractView(string contractID)
        {
            var contract = _sqlConnectionServer.HopDongs.Find(contractID);
            if(contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }
        public ActionResult Print(string contractID)
        {
            var contract = _sqlConnectionServer.HopDongs.Find(contractID);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }
    }
}