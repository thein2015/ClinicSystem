# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Project ဖိုင်တွေအားလုံးကို Docker ထဲ အရင် copy ကူးပါ
COPY . .

# Solution file (.sln) ရှိရင် အဲဒါကို restore လုပ်ပါ၊ မရှိရင် Web project ကိုပဲ restore လုပ်ပါ
RUN dotnet restore "ClinicSystem.Web/ClinicSystem.Web.csproj"

# Build လုပ်ပါ
RUN dotnet publish "ClinicSystem.Web/ClinicSystem.Web.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
