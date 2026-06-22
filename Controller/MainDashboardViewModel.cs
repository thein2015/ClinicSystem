using ClinicSystem.Web.Models;
using System.Net.Http.Json;

namespace ClinicSystem.Web.Controller
{
    public class DashboardService
    {
        private readonly HttpClient _httpClient;
        public DashboardService(HttpClient httpClient) => _httpClient = httpClient;
        public async Task<IncomeApiResponse?> GetIncomeReportAsync()
        {
            // သင်လိုချင်သည့် API URL အပြည့်အစုံကို ထည့်ပါ
            string url = "https://smartlivingmyanmar.com/CtlClinic1/api/get/IncomeSummary/RegK_4";

            try
            {
                // API မှ Data ကို GET method ဖြင့် ခေါ်ယူခြင်း
                return await _httpClient.GetFromJsonAsync<IncomeApiResponse>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return null;
            }
        }
    }
}
