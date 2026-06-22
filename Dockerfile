# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Repository တစ်ခုလုံးကို Docker ထဲသို့ ကူးယူပါ
COPY . .

# Shared ပရောဂျက်ကို အရင် build လုပ်ပါ
# သင့် folder နာမည်အတိုင်း ပြင်ပါ (အကယ်၍ folder နာမည် ClinicSystem.Shared မဟုတ်ရင် အမှန်အတိုင်းပြင်ပါ)
RUN dotnet restore "ClinicSystem.Shared/ClinicSystem.Shared.csproj"
RUN dotnet build "ClinicSystem.Shared/ClinicSystem.Shared.csproj" -c Release

# Web ပရောဂျက်ကို build လုပ်ပါ
RUN dotnet restore "ClinicSystem.Web/ClinicSystem.Web.csproj"
RUN dotnet publish "ClinicSystem.Web/ClinicSystem.Web.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
