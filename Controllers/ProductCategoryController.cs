using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Manament_Store_API.Controllers
{
    public class ProductCategoryController : Controller
    {

        private readonly SqlConnectionServer _sqlConnectionServer;

        public ProductCategoryController()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        // GET: ProductCategory
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProductCategories()
        {
            var categories = _sqlConnectionServer.DanhMucSanPhams.AsEnumerable().Select(c => new
            {
                c.MaDanhMucSP,
                c.TenDanhMuc,
                c.MoTa,
                NgayvaGio = c.NgayvaGio.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();
            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchCodeCategory(string codeCategory)
        {
            try
            {
                Console.WriteLine("Dữ liệu nhận từ AJAX: " + codeCategory);
                if (string.IsNullOrEmpty(codeCategory))
                {
                    return Json(new { success = false, message = "Mã danh mục không hợp lệ!" });
                }
                var category = _sqlConnectionServer.DanhMucSanPhams.FirstOrDefault(d => d.MaDanhMucSP == codeCategory);
                if(category == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy mã danh mục cần tìm" });
                }
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        MaDanhMucSP = category.MaDanhMucSP,
                        TenDanhMuc = category.TenDanhMuc,
                        MoTa = category.MoTa,
                        NgayvaGio = category.NgayvaGio.ToString("yyyy-MM-ddTHH:mm:ss")
                    }

                });


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi:{ex.Message}" });
            }
        }
        [HttpGet]
        public ActionResult AddProductCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProductCategory(string categoryName, string describe, DateTime Datecreated)
        {
            try
            {
                if (string.IsNullOrEmpty(categoryName))
                {
                    return Json(new { success = false, message = "Tên danh mục không được để trống" });
                }
                var isDate = new Service.IsDate();
                if (!isDate.IsValidDateTime(Datecreated))
                {
                    return Json(new { success = false, message = "Không thể chọn ngày giờ trong quá khứ!" });
                }
                if (string.IsNullOrEmpty(describe))
                {
                    return Json(new { success = false, message = "Vui lòng viết mô tả về danh mục sản phẩm" });
                }
                var CodeCategory = new Service.ProductCategory();
                string Categorycode = CodeCategory.GenerateCategoryCode(categoryName);
                var newProductCategory = new DanhMucSanPham
                {
                    MaDanhMucSP = Categorycode,
                    TenDanhMuc = categoryName,
                    NgayvaGio = Datecreated,
                    MoTa = describe
                };
                _sqlConnectionServer.DanhMucSanPhams.Add(newProductCategory);
                _sqlConnectionServer.SaveChanges();
                return Json(new { success = true, message = "Thêm danh mục sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi:{ex.Message}" });
            }
        }
        // GET: UpdateProductCategory
        [HttpGet]
        public ActionResult UpdateProductCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateProductCategory(string codeCategoryID, DanhMucSanPham categoryUpdate)
        {
            try
            {
                if (string.IsNullOrEmpty(codeCategoryID))
                {
                    return Json(new { success = false, message = "Vui lòng nhập mã tìm kiếm" });
                }
                var Category = _sqlConnectionServer.DanhMucSanPhams.FirstOrDefault(d => d.MaDanhMucSP == codeCategoryID);
                if (Category == null)
                {
                    return Json(new { success = false, message = "Mã danh mục không tồn tại" });
                }
                if (string.IsNullOrEmpty(categoryUpdate.TenDanhMuc))
                {
                    return Json(new { success = false, message = "Tên danh mục không được để trống" });
                }
                if (string.IsNullOrEmpty(categoryUpdate.MoTa))
                {
                    return Json(new { success = false, message = "Vui lòng viết mô tả về sản phẩm" });
                }
                var isDate = new Service.IsDate();
                if (!isDate.IsValidDateTime(categoryUpdate.NgayvaGio))
                {
                    return Json(new { success = false, message = "vui lòng không chọn ngày quá khứ" });
                }
                Category.TenDanhMuc = categoryUpdate.TenDanhMuc;
                Category.MoTa = categoryUpdate.MoTa;
                Category.NgayvaGio = categoryUpdate.NgayvaGio;
                _sqlConnectionServer.SaveChanges();

                return Json(new { success = true, message = "Cập nhật danh mục sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi:{ex.Message}" });
            }
        }
        [HttpGet]
        public ActionResult DeleteProductCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteProductCategory(string codeCategoryID)
        {

        }
    }
}