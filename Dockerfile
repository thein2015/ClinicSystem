# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# ဖိုင်တွေအားလုံးကို ကူးယူပါ
COPY . .

# Solution ဖိုင်ကို မသုံးဘဲ Shared Project ကို အရင် Restore လုပ်ပါ
# (အကယ်၍ Folder နာမည်မှားနေရင် အဲဒါကိုသာ ပြင်ပေးပါ)
RUN dotnet restore "ClinicSystem.Shared/ClinicSystem.Shared.csproj"
RUN dotnet restore "ClinicSystem.Web/ClinicSystem.Web.csproj"

# Build နှင့် Publish လုပ်ပါ
RUN dotnet publish "ClinicSystem.Web/ClinicSystem.Web.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
