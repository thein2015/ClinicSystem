# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# ပထမဆုံး Solution ဖိုင် (.sln) ကို အရင် copy ကူးပါ
COPY *.sln ./
COPY *.csproj ./

# Restoration အဆင့်
RUN dotnet restore

# ကျန်တဲ့ဖိုင်တွေအားလုံးကို ကူးပြီး Build လုပ်ပါ
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
