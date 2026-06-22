# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Repository တစ်ခုလုံးကို Docker ထဲသို့ ကူးယူပါ
COPY . .

# ပရောဂျက်အားလုံးကို Restore နှင့် Publish လုပ်ပါ
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
