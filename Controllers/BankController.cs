using Manament_Store_API.Models;
using Manament_Store_API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Manament_Store_API.Controllers
{
    public class BankController : Controller
    {
        private readonly string _apiKey = "852f8450-5762-4c86-a340-8d0b34c61db1key";
        private readonly string _apiSecret = "38a40aa8-07c4-4a10-b764-d801c09fb050secret";
        private readonly string _baseUrl = "https://api.banklookup.net/api/bank";


        // Hiển thị form nhập liệu và danh sách ngân hàng
        public async Task<ActionResult> AddAccountBankSupplier()
        {
            try
            {
                // Gọi API để lấy danh sách ngân hàng
                var bankList = await GetBankListAsync();
                ViewBag.BankList = bankList; // Truyền danh sách ngân hàng sang View
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi lấy danh sách ngân hàng: {ex.Message}";
            }

            return View();
        }

        // Xử lý form và hiển thị kết quả
        [HttpPost]
        public async Task<ActionResult> AddAccountBankSupplier(string bankCode, string accountNumber)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Thêm headers
                    client.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                    client.DefaultRequestHeaders.Add("x-api-secret", _apiSecret);

                    // Tạo payload
                    var payload = new
                    {
                        bank = bankCode,
                        account = accountNumber
                    };
                    string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                    HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Gọi API kiểm tra tài khoản
                    HttpResponseMessage response = await client.PostAsync($"{_baseUrl}/id-lookup-prod", content);

                    // Kiểm tra kết quả
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(result);




                        ViewBag.OwnerName = jsonResponse.data.ownerName;
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ViewBag.Error = $"Lỗi: {response.StatusCode}";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi: {ex.Message}";
            }
            // Gọi lại API danh sách ngân hàng để hiển thị dropdown
            try
            {
                var bankList = await GetBankListAsync();
                ViewBag.BankList = bankList;
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi lấy danh sách ngân hàng: {ex.Message}";
            }

            return View();

        }


        private async Task<List<BankInfo>> GetBankListAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                // Thêm headers
                client.DefaultRequestHeaders.Add("x-api-key", _apiKey);
                client.DefaultRequestHeaders.Add("x-api-secret", _apiSecret);

                // Gọi API
                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/list");

                // Kiểm tra kết quả
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON vào model BankApiResponse
                    var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<BankApiResponse>(result);

                    // Trả về danh sách ngân hàng
                    return apiResponse.Data;
                }
                else
                {
                    throw new Exception($"Lỗi: {response.StatusCode}");
                }
            }
        }
    }
}
