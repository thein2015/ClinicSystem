# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# ဖိုင်တွေအားလုံးကို copy ကူးပါ
COPY . .

# Solution file (.sln) ကို အသုံးပြုပြီး Build လုပ်ခြင်း
# အကယ်၍ error ထပ်တက်ရင် အောက်က line ကိုသုံးပါ
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
