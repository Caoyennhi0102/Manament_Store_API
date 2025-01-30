using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Manament_Store_API.Service
{
    public class UserNameRandom
    {
        // Hàm tạo chuỗi ngẫu nhiên
        public string GenerateRandomString(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=";
            StringBuilder randomString = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                randomString.Append(validChars[random.Next(validChars.Length)]);
            }
            return randomString.ToString();
        }
    }
}