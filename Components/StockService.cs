using ClinicSystem.Web.Controller;
using ClinicSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using static PayPal.BaseConstants;

namespace ClinicSystem.Web.Components
{
    public class StockService
    {
        private readonly HttpClient _httpClient;

        // Inject HttpClient via constructor
        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        

        public async Task<List<StockBalanceItem>> GetStockDataAsync(string mode, string keyy, string dbName)
        {
            string apiUrl = $"https://smartlivingmyanmar.com/{dbName}/api/get/{mode}";
            var payload = new { Keyy = keyy };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);
            if (!response.IsSuccessStatusCode) throw new Exception($"API Error: {response.StatusCode}");

            string json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.GetProperty("status").GetInt32() != 0)
                throw new Exception(root.GetProperty("msg").GetString());

            return root.GetProperty("result").EnumerateArray()
                       .Select(item => MapJsonToModel(item, mode))
                       .ToList();
        }

        private StockBalanceItem MapJsonToModel(JsonElement item, string mode)
        {
            var model = new StockBalanceItem
            {
                No = item.TryGetProperty("No", out var n) ? n.GetInt32() : 0,
                ItemName = item.TryGetProperty("ItemName", out var name) ? name.GetString() : "Unknown",
                ItemId = item.TryGetProperty("ItemId", out var id) ? id.GetInt32() : 0
            };

            if (mode == "StockPrices" && item.TryGetProperty("Price", out var p))
            {
                string[] parts = p.GetString().Split(',');
                if (parts.Length >= 3)
                {
                    var norm = parts[0].Split('@');
                    model.NormalPrice = double.TryParse(norm[0], out var np) ? np : 0;
                    model.NormPk = norm.Length > 1 ? double.Parse(norm[1]) : 0;

                    var mem = parts[1].Split('@');
                    model.MemberPrice = double.TryParse(mem[0], out var mp) ? mp : 0;
                    model.MemPk = mem.Length > 1 ? double.Parse(mem[1]) : 0;

                    var work = parts[2].Split('@');
                    model.WorkInPrice = double.TryParse(work[0], out var wp) ? wp : 0;
                    model.WorkPk = work.Length > 1 ? double.Parse(work[1]) : 0;
                }
                model.IsPriceMode = true;
            }
            else if (mode == "ServicePrices")
            {
                // Map the properties directly from the JSON
                model.NormalPrice = item.TryGetProperty("NormalPrice", out var np) ? np.GetDouble() : 0;
                model.MemberPrice = item.TryGetProperty("MemberPrice", out var mp) ? mp.GetDouble() : 0;
                model.WorkInPrice = item.TryGetProperty("WorkInPrice", out var wp) ? wp.GetDouble() : 0;

                // Services might not have "Pack" info, so default to 0 or 1
                model.NormPk = 0;
                model.MemPk = 0;
                model.WorkPk = 0;

                model.IsPriceMode = true;
            }
            else model.Balance = item.TryGetProperty("Balance", out var b) ? b.GetInt32() : 0;

            return model;
        }
        
        public async Task<bool> UpdatePricesToApiAsync(int itemId, double mem, double norm, double walk, bool isService = false)
        {
            try
            {
                string currentKey = DBController.Keyy;
                string dbName = "CtlClinic1"; // Ensure this matches your active database logic

                // Dynamic URL based on whether it's a Service or Stock
                string endpoint = isService ? "UpdateServicePrices" : "UpdatePrices";
                string apiUrl = $"https://smartlivingmyanmar.com/{dbName}/api/update/{endpoint}";

                var payload = new
                {
                    Keyy = currentKey,
                    ItemId = itemId,
                    MemberPrice = mem,
                    NormalPrice = norm,
                    WorkInPrice = walk
                    // Add Pk fields here if your backend API expects them for Services too
                };

                string jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);
                var httpContent = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(apiUrl, httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonRes = await response.Content.ReadAsStringAsync();
                        using (JsonDocument doc = JsonDocument.Parse(jsonRes))
                        {
                            return doc.RootElement.GetProperty("status").GetInt32() == 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
            }
            return false;
        }
        public async Task<bool> PostPriceUpdateAsync(int itemId, double p1, double p2, double p3, double p1pk, double p2pk, double p3pk)
        {
            // Use the base URL or inject the client properly
            string url = "https://smartlivingmyanmar.com/CtlClinic1/api/update/UpdateSellPrices";

            var payload = new
            {
                ItemId = itemId,
                p1 = p1,
                p2 = p2,
                p3 = p3,
                p1pk = p1pk,
                p2pk = p2pk,
                p3pk = p3pk,
                Keyy = "RegK_4"
            };

            try
            {
                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                Console.WriteLine($"API Error: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return false;
            }
        }
        //    public async Task<List<StockBalanceItem>> GetStockDataAsync(string mode, string keyy, string dbName)
        //    {
        //        string apiUrl = $"https://smartlivingmyanmar.com/{dbName}/api/get/{mode}";

        //        // Use FormUrlEncodedContent to match Postman's behavior exactly
        //        var content = new FormUrlEncodedContent(new[]
        //        {
        //    new KeyValuePair<string, string>("Keyy", keyy)
        //});

        //        // Send the request
        //        var response = await _httpClient.PostAsync(apiUrl, content);

        //        if (!response.IsSuccessStatusCode)
        //            throw new Exception($"API Connection Error: {response.StatusCode}");

        //        var json = await response.Content.ReadAsStringAsync();

        //        // Debugging: If it still fails, uncomment the line below to see what the server is actually sending back
        //        // Console.WriteLine("Server Response: " + json);

        //        using var doc = JsonDocument.Parse(json);
        //        var root = doc.RootElement;

        //        // Check status
        //        if (root.TryGetProperty("status", out var status) && status.GetInt32() != 0)
        //            throw new Exception(root.TryGetProperty("msg", out var msg) ? msg.GetString() : "Unknown Error");

        //        // Map result
        //        if (root.TryGetProperty("result", out var resultElement) && resultElement.ValueKind == JsonValueKind.Array)
        //        {
        //            return resultElement.EnumerateArray()
        //                .Select(item => MapJsonToModel(item, mode))
        //                .ToList();
        //        }

        //        return new List<StockBalanceItem>();
        //    }

        public async Task<List<StockBalanceItem>> GetStockDataAsync1(string mode, string keyy, string dbName)
        {
            string apiUrl = $"https://smartlivingmyanmar.com/{dbName}/api/get/{mode}";

            // 1. Create a C# anonymous object that matches your JSON structure {"Keyy":"RegK_3"}
            var payload = new { Keyy = keyy };

            // 2. Serialize it to a JSON string
            string jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);

            // 3. Send as application/json
            var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"API Connection Error: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();

            // 4. Parse response
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.GetProperty("status").GetInt32() != 0)
                throw new Exception(root.GetProperty("msg").GetString());

            return root.GetProperty("result").EnumerateArray()
                       .Select(item => MapJsonToModel(item, mode))
                       .ToList();

        }
        //public async Task<List<StockBalanceItem>> GetStockDataAsync(string mode, string keyy, string dbName)
        //{
        //    string apiUrl = $"https://smartlivingmyanmar.com/{dbName}/api/get/{mode}";

        //    // 1. Explicitly define the payload with the exact casing expected by the API
        //    var payload = new { Keyy = keyy };

        //    // 2. Send the request
        //    var response = await _httpClient.PostAsJsonAsync(apiUrl, payload);

        //    // 3. Handle non-success status codes (e.g., 404, 500)
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        var errorContent = await response.Content.ReadAsStringAsync();
        //        throw new Exception($"API Connection Error ({response.StatusCode}): {errorContent}");
        //    }

        //    // 4. Parse the response securely
        //    var jsonResponse = await response.Content.ReadFromJsonAsync<JsonDocument>();

        //    if (jsonResponse == null)
        //        throw new Exception("Received empty response from server.");

        //    var root = jsonResponse.RootElement;

        //    // 5. Safely check for "status" property
        //    if (root.TryGetProperty("status", out var statusProp) && statusProp.GetInt32() != 0)
        //    {
        //        string msg = root.TryGetProperty("msg", out var msgProp) ? msgProp.GetString() : "Unknown error occurred.";
        //        throw new Exception(msg);
        //    }

        //    // 6. Map the results
        //    if (root.TryGetProperty("result", out var resultElement) && resultElement.ValueKind == JsonValueKind.Array)
        //    {
        //        return resultElement.EnumerateArray()
        //            .Select(item => MapJsonToModel(item, mode))
        //            .ToList();
        //    }

        //    return new List<StockBalanceItem>();
        //}
        //private StockBalanceItem MapJsonToModel(JsonElement jsonItem, string mode)
        //{
        //    var model = new StockBalanceItem
        //    {
        //        No = jsonItem.TryGetProperty("No", out var n) ? n.GetInt32() : 0,
        //        ItemName = jsonItem.TryGetProperty("ItemName", out var name) ? name.GetString() : "Unknown",
        //        ItemId = jsonItem.TryGetProperty("ItemId", out var id) ? id.GetInt32() : 0
        //    };

        //    // Mapping logic based on mode
        //    if (mode == "ServicePrices")
        //    {
        //        model.MemberPrice = jsonItem.TryGetProperty("MemberPrice", out var m) ? m.GetDouble() : 0;
        //    }
        //    else
        //    {
        //        model.Balance = jsonItem.TryGetProperty("Balance", out var b) ? b.GetInt32() : 0;
        //    }

        //    return model;
        //}
        private StockBalanceItem MapJsonToModel1(JsonElement jsonItem, string mode)
        {
            var modelItem = new StockBalanceItem
            {
                No = jsonItem.TryGetProperty("No", out var no) ? no.GetInt32() : 0,
                ItemName = jsonItem.TryGetProperty("ItemName", out var name) ? name.GetString() : "Unknown",
                ItemId = jsonItem.TryGetProperty("ItemId", out var id) ? id.GetInt32() : 0
            };

            // Branching logic based on the 'mode'
            if (mode == "ServicePrices")
            {
                modelItem.MemberPrice = jsonItem.TryGetProperty("MemberPrice", out var mem) ? mem.GetDouble() : 0.0;
                modelItem.NormalPrice = jsonItem.TryGetProperty("NormalPrice", out var norm) ? norm.GetDouble() : 0.0;
                modelItem.WorkInPrice = jsonItem.TryGetProperty("WorkInPrice", out var work) ? work.GetDouble() : 0.0;
                modelItem.CountStyle = jsonItem.TryGetProperty("CountStyle", out var style) ? style.GetString() : "";
                modelItem.ActionText = jsonItem.TryGetProperty("ActionText", out var act) ? act.GetString() : "Set Price";
                modelItem.Balance = 0;
            }
            else if (mode == "StockBalance")
            {
                modelItem.Balance = jsonItem.TryGetProperty("Balance", out var b) ? b.GetInt32() : 0;
            }

            return modelItem;
        }

        public async Task<bool> PostPriceUpdateAsync(StockBalanceItem item)
        {
            string url = "https://smartlivingmyanmar.com/CtlClinic1/api/update/UpdateSellPrices";

            var payload = new
            {
                ItemId = item.ItemId,
                p1 = item.MemberPrice,
                p2 = item.NormalPrice,
                p3 = item.WorkInPrice,
                p1pk = item.MemPk,
                p2pk = item.NormPk,
                p3pk = item.WorkPk,
                Keyy = "RegK_4"
            };

            try
            {
                // PostAsJsonAsync handles serialization and Content-Type headers automatically
                var response = await _httpClient.PostAsJsonAsync(url, payload);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                Console.WriteLine($"API returned error: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request exception: {ex.Message}");
                return false;
            }
        }
    }
}
