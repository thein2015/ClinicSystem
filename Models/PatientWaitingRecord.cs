using System.Text.Json.Serialization;

namespace ClinicSystem.Web.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("data")]
        public List<PatientWaitingRecord> Data { get; set; }
        //[JsonPropertyName("status")]
        //public int Status { get; set; }

        //[JsonPropertyName("msg")]
        //public string Msg { get; set; }

        //[JsonPropertyName("data")]
        //public List<PatientWaitingRecord> Data { get; set; } // ဒီနေရာမှာ Data ဆိုတဲ့ List ရှိရပါမယ်
    }

    public class PatientWaitingRecord
    {
        [JsonPropertyName("PatientId")]
        public int? PatientId { get; set; } // ID သည် ဂဏန်းဖြစ်သောကြောင့် int? သုံးပါ

        [JsonPropertyName("Patient Name")]
        public string? PatientName { get; set; }

        [JsonPropertyName("Birth of Date")]
        public string? BirthOfDate { get; set; }

        [JsonPropertyName("Gender")]
        public string? Gender { get; set; }

        [JsonPropertyName("Age")]
        public int? Age { get; set; } // Age မရှိနိုင်သည်ကို ထည့်တွက်၍ int? သုံးပါ

        [JsonPropertyName("Case")]
        public string? Case { get; set; }

        [JsonPropertyName("Blood Type")]
        public string? BloodType { get; set; }

        [JsonPropertyName("Telephone")]
        public string? Telephone { get; set; }

        [JsonPropertyName("Address")]
        public string? Address { get; set; }

        [JsonPropertyName("PHistoryId")]
        public int? PHistoryId { get; set; }

        [JsonPropertyName("PHDate")]
        public string? PHDate { get; set; }

        [JsonPropertyName("PHweight")]
        public string? PHweight { get; set; }

        [JsonPropertyName("PHBP")]
        public string? PHBP { get; set; }

        [JsonPropertyName("PHTemp")]
        public string? PHTemp { get; set; }

        [JsonPropertyName("Doctor")]
        public string? Doctor { get; set; }

        [JsonPropertyName("Nurse")]
        public string? Nurse { get; set; }

        [JsonPropertyName("Alergy")]
        public string? Alergy { get; set; }

        [JsonPropertyName("Amountt")]
        public decimal? Amountt { get; set; } // decimal? သုံးခြင်းဖြင့် 0 တန်ဖိုးအတွက် အဆင်ပြေစေသည်

        [JsonPropertyName("Last Date")]
        public string? LastDate { get; set; }

        [JsonPropertyName("Qrcode")]
        public string? Qrcode { get; set; }
    }
}