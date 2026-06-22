# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Repository တစ်ခုလုံးကို Docker ထဲသို့ ကူးယူပါ
COPY . .

# Solution (.sln) ကို အသုံးပြုပြီး ပရောဂျက်အားလုံးကို Restore လုပ်ပါ
RUN dotnet restore "ClinicSystem.sln"

# Web project ကို publish လုပ်ပါ
# လမ်းကြောင်းအမှန်အတိုင်း ပြင်ပေးပါ (ဥပမာ- ClinicSystem.Web/ClinicSystem.Web.csproj)
RUN dotnet publish "ClinicSystem.Web/ClinicSystem.Web.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
