using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class PositionCode
    {
        public string CreatePositionCode(string TenCV)
        {
            // Tách chuỗi thành các từ, loại bỏ khoảng trắng thừa
            var words = TenCV.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Lấy ký tự đầu của mỗi từ và giữ nguyên chữ hoa/thường
            string MaCV = string.Concat(words.Select(w => w.Substring(0, 1)));

            // Thêm tiền tố "CV_"
            return "CV_" + MaCV;

        }
    }
}