# Build Stage (SDK ကို .NET 9.0 သို့ ပြောင်းပါ)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Repository တစ်ခုလုံးကို Docker ထဲသို့ ကူးယူပါ
COPY . .

# Web Project ကို restore လုပ်ပါ
RUN dotnet restore "ClinicSystem.Web.csproj"
RUN dotnet publish "ClinicSystem.Web.csproj" -c Release -o /app/publish

# Runtime Stage (Runtime ကိုလည်း .NET 9.0 သို့ ပြောင်းပါ)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
