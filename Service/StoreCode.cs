using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class StoreCode
    {
        private readonly SqlConnectionServer _sqlConnectionServer;
        public StoreCode()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        public int GenerateCuaHangId()
        {
            // Lấy mã cửa hàng cao nhất hiện tại
            var maxID = _sqlConnectionServer.CuaHangs.Select(CH => CH.MaCuaHang).DefaultIfEmpty(0).Max();


            // Tăng mã lên 1
            return maxID + 1;
        }
        // Phương thức định dạng mã cửa hàng 
        public string GetFormattedCuaHangId(int NewStoreID)
        {
            NewStoreID = GenerateCuaHangId();
            return "CH" + NewStoreID.ToString("D4");
        }
    }
}