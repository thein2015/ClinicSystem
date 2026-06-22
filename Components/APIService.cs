using System.Net.Http.Json;
using ClinicSystem.Web.Controller;
using Newtonsoft.Json;

namespace ClinicSystem.Web.Components;

public class APIService
{
    private readonly HttpClient _http;
    private readonly DBController _db; // Use instance

    public APIService(HttpClient http, DBController db)
    {
        _http = http;
        //_db = db;
    }

    public async Task<string> LoginToServerAsync(string user, string pass)
    {
        // Use _db.Dbse (instance) instead of DBController.Dbse (static)
        string apiUrl = $"https://smartlivingmyanmar.com/{Uri.EscapeDataString(DBController.Dbse ?? "")}/api/get/LoginUser";
        var payload = new { username = user, password = pass, Keyy = DBController.Keyy };

        try
        {
            var response = await _http.PostAsJsonAsync(apiUrl, payload);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new { status = 2, msg = "Connection error: " + ex.Message });
        }
    }
}