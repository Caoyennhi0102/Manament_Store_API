using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Manament_Store_API.Service
{
    public class PasswordRandom
    {
        // Hàm băm mật khẩu với Salt
        public (string HashedPassword, string Salt) HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var IsUserRandom = new Service.UserNameRandom();
                string salt = IsUserRandom.GenerateRandomString(16); // Tạo salt ngẫu nhiên
                hmac.Key = Encoding.UTF8.GetBytes(salt);
                var hashedPassword = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return (hashedPassword, salt);
            }
        }
    }
}