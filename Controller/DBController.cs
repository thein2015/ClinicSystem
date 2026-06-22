using ClinicSystem.Web.Models;
using System.Net.Http;
namespace ClinicSystem.Web.Controller
{
    public class DBController
    {
        public static List<PatientNameRecord> masterPatientList { get; set; } = new();
        // Pass the HttpClient into this method or store it in the class
        public static string RegKey = "1", Dbse= "CtlClinic1", Keyy = "RegK_4", SrvCloud = "f", SpecialItemcode = "", SpecialItemcode_M = "";
        public static bool Intr_Condi;
        public static int St_NumSalePrice = 0, ShopId = 1, CusTomerId, SpecialItemId, SpecialItemId_M, SpecialCost;
        public static string OfficeId, RegK, OffDb, CurrentVersion;
        private readonly HttpClient _httpClient = new HttpClient();

        // This constructor allows Blazor to pass the HttpClient to you
        public DBController()
        {

        }
        public async Task<List<PatientWaitingRecord>> LoadWaitingListFromPHPAsync(string username, int usertype, string clinicKey)
        {
            string apiUrl = "https://smartlivingmyanmar.com/CtlClinic1/api/get/GetPhRecordNotFinish";

            var formData = new Dictionary<string, string>
    {
        { "Username", username },
        { "UserType", usertype.ToString() },
        { "Keyy", clinicKey }
    };

            try
            {
                // Use the class-level _httpClient injected via constructor
                 
                var response = await _httpClient.PostAsync(apiUrl, new FormUrlEncodedContent(formData));
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    // Debug လုပ်ရန်အတွက် JSON ကို Console မှာ ကြည့်ပါ
                    Console.WriteLine("API Response JSON: " + json);

                    try
                    {
                        var apiResult = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(json,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        return apiResult?.Data ?? new List<PatientWaitingRecord>();
                    }
                    catch (System.Text.Json.JsonException ex)
                    {
                        Console.WriteLine("JSON Deserialize Error: " + ex.Message);
                        return new List<PatientWaitingRecord>();
                    }
                }
                //if (response.IsSuccessStatusCode)
                //{
                //    var json = await response.Content.ReadAsStringAsync();
                //    var apiResult = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(json,
                //        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                //    return apiResult?.Data ?? new List<PatientWaitingRecord>();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
            }
            return new List<PatientWaitingRecord>();
        }
        //public static async Task<List<PatientWaitingRecord>> LoadWaitingListFromPHPAsync(HttpClient http, string username, int usertype, string clinicKey)
        //{
        //    string apiUrl = "https://smartlivingmyanmar.com/CtlClinic1/api/get/GetPhRecordNotFinish";

        //    var formData = new Dictionary<string, string>
        //{
        //    { "Username", username },
        //    { "UserType", usertype.ToString() },
        //    { "Keyy", clinicKey }
        //};

        //    try
        //    {
        //        var response = await http.PostAsync(apiUrl, new FormUrlEncodedContent(formData));
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Use System.Text.Json to deserialize
        //            var json = await response.Content.ReadAsStringAsync();
        //            var apiResult = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //            return apiResult?.Data ?? new List<PatientWaitingRecord>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"API Error: {ex.Message}");
        //    }
        //    return new List<PatientWaitingRecord>();
        //}
    }
}