using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manament_Store_API.Models;

namespace Manament_Store_API.Controllers.Admin
{
   // [Authorize(Roles ="Admin")]
    public class ImageSettingsController : Controller
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public ImageSettingsController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: Hiển thị danh sách ảnh
        public ActionResult Index()
        {
            var images = _sqlConnectionServer.ImageSettings.ToList();

            return View(images);
        }
        // GET: Form cập nhật ảnh
        [HttpGet]
        public ActionResult EditImage(string imageType)
        {
            var image = _sqlConnectionServer.ImageSettings.FirstOrDefault(i => i.DuLieuAnh == imageType);
            return View(image);
        }
        [HttpPost]
        public ActionResult EditImage(string imageType, HttpPostedFileBase imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.ContentLength == 0)
                {
                    ModelState.AddModelError("", "Vui lòng chọn ảnh.");
                    return View();
                }

                // Validate định dạng ảnh
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Chỉ chấp nhận file JPG/PNG.");
                    return View();
                }

                // Lấy tên file gốc
                var fileName = Path.GetFileName(imageFile.FileName);

                // Tạo đường dẫn lưu file
                var path = Path.Combine(Server.MapPath("~/Image/Uploads"), fileName);
                if (System.IO.File.Exists(path))
                {
                    fileName = $"{Path.GetFileNameWithoutExtension(imageFile.FileName)}_{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                    path = Path.Combine(Server.MapPath("~/Image/Uploads"), fileName);
                }
                // Kiểm tra đường dẫn thư mục
                var directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                imageFile.SaveAs(path);

                // Cập nhật database
                var imageSetting = _sqlConnectionServer.ImageSettings.FirstOrDefault(i => i.DuLieuAnh == imageType);
                if (imageSetting == null)
                {
                    imageSetting = new ImageSetting
                    {
                        DuLieuAnh = imageType ?? "default",
                        DuongDanAnh = $"/Image/Uploads/{fileName}",
                        NgayThay = DateTime.Now
                    };
                    _sqlConnectionServer.ImageSettings.Add(imageSetting);

                }
                else
                {
                      imageSetting.DuLieuAnh = imageType ?? "default";
                    imageSetting.DuongDanAnh = $"//Uploads/{fileName}";
                    imageSetting.NgayThay = DateTime.Now;
                }
                _sqlConnectionServer.SaveChanges();
                return RedirectToAction("EditImage", new {imageType});
            }catch(Exception ex)
            {
                ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
                return View();
            }
        }

    }
}