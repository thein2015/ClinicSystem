# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# ဖိုင်တွေအားလုံးကို copy ကူးပါ
COPY . .

# အရေးကြီးဆုံး: မည်သည့် path မှမပါဘဲ restore လုပ်ပါ
# Docker က သူတွေ့သမျှ csproj ဖိုင်တွေကို အလိုအလျောက်ရှာပြီး restore လုပ်ပါလိမ့်မယ်
RUN dotnet restore

# Build နှင့် Publish လုပ်ပါ
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
