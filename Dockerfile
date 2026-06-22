# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Repository တစ်ခုလုံးကို Docker ထဲသို့ ကူးယူပါ
COPY . .

# Solution (.sln) ဖိုင်မရှိသော်လည်း Project များအားလုံးကို အောင်မြင်စွာ Restore လုပ်နိုင်ရန်
# Web Project ကို ဦးစားပေးပြီး Shared ကိုပါ ချိတ်ဆက်ပေးမည့် command
RUN dotnet restore "ClinicSystem.Web.csproj"

# Web Project ကို Release mode ဖြင့် publish လုပ်ပါ
RUN dotnet publish "ClinicSystem.Web.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
