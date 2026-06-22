using System.Text.Json.Serialization;
using System.Text;

namespace ClinicSystem.Web.Models
{
    public class GeneralModel
    {

    }
    //public class IncomeApiResponse
    //{
    //    public int Status { get; set; }
    //    public string Msg { get; set; } = string.Empty;
    //    // Result property ကို သေချာပေါက် သတ်မှတ်ပေးပါ
    //    public IncomeData Result { get; set; } = new();
    //}

    public class IncomeData
    {
        public double SellAmount { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public double GrandTotal { get; set; }
    }
    public class PhpHistoryResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("history")]
        public List<PatientHistoryItem> History { get; set; }
    }

    public class PatientHistoryItem
    {
        [JsonPropertyName("patientId")]
        public int PatientId { get; set; }

        [JsonPropertyName("phDate")]
        public string PhDate { get; set; }

        [JsonPropertyName("pHistoryId")]
        public int PHistoryId { get; set; }

        [JsonPropertyName("plann")]
        public string Plann { get; set; }

        [JsonPropertyName("pComplain")]
        public string PComplain { get; set; }

        [JsonPropertyName("diagnosisInfo")]
        public string DiagnosisInfo { get; set; }

        [JsonPropertyName("medicineInfo")]
        public string MedicineInfo { get; set; }

        [JsonPropertyName("remark")]
        public string Remark { get; set; }

        [JsonPropertyName("posInfo")]
        public string PosInfo { get; set; }

        [JsonPropertyName("diagnosisList")]
        public List<string> DiagnosisList { get; set; }

        [JsonPropertyName("medicineList")]
        public List<string> MedicineList { get; set; }

        [JsonPropertyName("posList")]
        public List<string> PosList { get; set; }

        // ==============================================================
        // 🎯 ဒေတာအားလုံးကို စာတန်းတစ်ခုတည်းဖြစ်အောင် စုစည်းပေးမည့် နေရာ
        // ==============================================================
        [System.Text.Json.Serialization.JsonIgnore]
        public string DisplayAllHistory
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                // ၁။ Diagnosis ရှိရင် ထည့်မည်
                sb.AppendLine("📋 [Diagnosis Info]");
                if (DiagnosisList != null && DiagnosisList.Count > 0)
                    sb.AppendLine(string.Join("\n", DiagnosisList));
                else if (!string.IsNullOrWhiteSpace(DiagnosisInfo))
                    sb.AppendLine(DiagnosisInfo);
                else
                    sb.AppendLine("No diagnosis record");

                sb.AppendLine(); // လိုင်းအလွတ်တစ်ကြောင်းခြားမည်

                // ၂။ Plan ရှိရင် ထည့်မည်
                if (!string.IsNullOrWhiteSpace(Plann))
                {
                    sb.AppendLine("📝 [Plan]");
                    sb.AppendLine(Plann);
                    sb.AppendLine();
                }

                // ၃။ ဆေးညွှန်း (Medicine) ရှိရင် ထည့်မည်
                sb.AppendLine("💊 [Medicine Info]");
                if (MedicineList != null && MedicineList.Count > 0)
                    sb.AppendLine(string.Join("\n", MedicineList));
                else if (!string.IsNullOrWhiteSpace(MedicineInfo))
                    sb.AppendLine(MedicineInfo.Replace("@", "\n"));
                else
                    sb.AppendLine("No prescription");

                sb.AppendLine();

                // ၄။ ဝယ်ယူမှု (POS/Treatments) ရှိရင် ထည့်မည်
                sb.AppendLine("💰 [POS Info]");
                if (PosList != null && PosList.Count > 0)
                    sb.AppendLine(string.Join("\n", PosList));
                else if (!string.IsNullOrWhiteSpace(PosInfo))
                    sb.AppendLine(PosInfo.Replace("@", "\n"));
                else
                    sb.AppendLine("No purchase record");

                // ၅။ Remark ရှိရင် ထည့်မည်
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    sb.AppendLine();
                    sb.AppendLine("ℹ️ [Remarks]");
                    sb.AppendLine(Remark);
                }

                return sb.ToString().TrimEnd();
            }
        }
    }
    public class PatientApiResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("data")]
        public List<PatientNameRecord> Data { get; set; }
    }

    public class PatientNameRecord
    {
        [JsonPropertyName("PatientId")]
        public int PatientId { get; set; }

        [JsonPropertyName("PName")]
        public string PatientName { get; set; }

        [JsonPropertyName("Bdate")]
        public string BirthOfDate { get; set; }

        [JsonPropertyName("Sex")]
        public string Gender { get; set; }

        [JsonPropertyName("AgeYears")]
        public int Age { get; set; }

        [JsonPropertyName("Phone")]
        public string Telephone { get; set; }

        [JsonPropertyName("PAddress")]
        public string Address { get; set; }

        [JsonPropertyName("Alergy")]
        public string Alergy { get; set; }

        [JsonPropertyName("BoodType")]
        public string BloodType { get; set; }

        [JsonPropertyName("Qrcode")]
        public string Qrcode { get; set; }
    }
    public class StockBalanceItem
    {
        // For Web, simple properties are faster and sufficient.
        // Blazor uses StateHasChanged() to refresh the UI.
        public bool IsPriceMode { get; set; }
        public int No { get; set; }
        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public int Balance { get; set; }

        public double MemberPrice { get; set; }
        public double NormalPrice { get; set; }
        public double WorkInPrice { get; set; }

        public double MemPk { get; set; }
        public double NormPk { get; set; }
        public double WorkPk { get; set; }

        public string CountStyle { get; set; }
        public string ActionText { get; set; }
    }
    public class IncomeApiResponse
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public IncomeResult Result { get; set; }
    }

    public class IncomeResult
    {
        public double Amount { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public double GrandTotal { get; set; }
    }
   
    public class IncomeDataResult
    {
        [Newtonsoft.Json.JsonProperty("SellAmount")]
        public double SellAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("Discount")]
        public double Discount { get; set; }

        [Newtonsoft.Json.JsonProperty("Total")]
        public double Total { get; set; }

        [Newtonsoft.Json.JsonProperty("GrandTotal")]
        public double GrandTotal { get; set; }
    }
}
