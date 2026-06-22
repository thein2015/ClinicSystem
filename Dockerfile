# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# ဖိုင်များအားလုံးကို copy ကူးပါ
COPY . .

# Solution file ရှိလျှင် Restore လုပ်ပါ၊ မရှိလျှင် စနစ်အလိုက် ရှာဖွေစေပါ
# အကယ်၍ error ထပ်တက်ပါက အောက်ပါအတိုင်း ပြင်ပါ
RUN dotnet restore "ClinicSystem.Web/ClinicSystem.Web.csproj"

# Build လုပ်ပါ
RUN dotnet publish "ClinicSystem.Web/ClinicSystem.Web.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
