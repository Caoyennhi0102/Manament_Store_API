using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class ProductCategory
    {
        public string GenerateCategoryCode(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return "Tên danh mục không được để trống";
            }
            string[] words = categoryName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string initials = string.Concat(words.Select(w => w[0])).ToUpper();

            return $"NH_{initials}";
        }
    }
}