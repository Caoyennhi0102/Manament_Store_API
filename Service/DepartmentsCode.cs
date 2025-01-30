using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Manament_Store_API.Service
{
    public class DepartmentsCode
    {
        public string CreateCodeDepartments(string TenBP)
        {
            if (string.IsNullOrEmpty(TenBP))
            {
                return "Tên bộ phận không được để trống.";
            }
            TenBP = RemoveDiacritics(TenBP.Trim());
            string[] form = TenBP.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string FirstCharacter = string.Concat(form.Select(t => t[0].ToString().ToUpper()));
            return $"BP_{FirstCharacter}";
        }
        // Phương thức để bỏ dấu tiếng Việt
        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var character in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}