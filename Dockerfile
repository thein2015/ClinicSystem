# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Repository တစ်ခုလုံးကို Docker ထဲသို့ ကူးယူပါ
COPY . .

# Solution file (.sln) ကို အခြေခံပြီး Dependencies အားလုံးကို Restore လုပ်ပါ
# အကယ်၍ သင့် Root မှာ .sln ဖိုင်မရှိရင် သူက သူ့ဘာသာ ရှာပါလိမ့်မယ်
RUN dotnet restore

# Web project ကို publish လုပ်ပါ
RUN dotnet publish ClinicSystem.Web/ClinicSystem.Web.csproj -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
