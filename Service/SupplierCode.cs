using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Manament_Store_API.Service
{
    public class SupplierCode
    {
        public string GenerateSupplierCode(string supplierName)
        {
            if (string.IsNullOrEmpty(supplierName))
            {
                return "Tên nhà cung cấp không được để trống";

            }
            string[] words = Regex.Split(supplierName.Trim(), @"\s+");

            string initials = string.Concat(words.Select(word => char.ToUpper(word[0])));

            return $"NCC_{initials}";
        }
    }
}