using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class InternalEmail
    {
        public string GenerateEmployeeEmail(string hoTen, int maNhanVien)
        {
            if (string.IsNullOrWhiteSpace(hoTen))
            {
                throw new ArgumentException("Họ tên không được để trống.", nameof(hoTen));
            }
            if (maNhanVien <= 0)
            {
                throw new ArgumentException("Mã nhân viên phải là số dương.", nameof(maNhanVien));
            }

            // Loại bỏ dấu tiếng Việt và chuyển họ tên thành chữ thường
            string Fullnamewithoutaccents = RemoveDiacritics(hoTen).ToLower();

            Fullnamewithoutaccents = string.Join("", Fullnamewithoutaccents.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            string email = $"{Fullnamewithoutaccents}{maNhanVien}@gmail.com";

            return email;

        }

        // Phương thức loại bỏ dấu tiếng Việt
        private string RemoveDiacritics(string text)
        {
            string normalizedText = text.Normalize(System.Text.NormalizationForm.FormD);
            var chars = normalizedText
                .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                .ToArray();

            return new string(chars).Normalize(System.Text.NormalizationForm.FormC);
        }
    }
}