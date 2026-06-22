# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Repository ထဲက ဖိုင်တွေအားလုံးကို copy ကူးပါ
COPY . .

# Solution file (.sln) ကို အခြေခံပြီး build လုပ်ပါ
# အကယ်၍ သင့် repository မှာ .sln ဖိုင်မရှိရင် ဒီအဆင့်ကို ကျော်ပြီး 
# တိုက်ရိုက် ClinicSystem.Web.csproj ကိုပဲ restore လုပ်ပါ
RUN dotnet restore "ClinicSystem.Web.csproj"
RUN dotnet publish "ClinicSystem.Web.csproj" -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
