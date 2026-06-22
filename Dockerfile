# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# အရေးကြီးဆုံးအချက်: ဖိုင်တွေအားလုံးကို အရင် copy ကူးပါ
COPY . .

# Solution file ရှိရင် အဲဒါကိုသုံးပြီး build လုပ်ပါ (မရှိရင် Web ကိုပဲ restore လုပ်ပါ)
# သင့် project တည်ဆောက်ပုံအရ Shared ဖိုင်တွဲကိုပါ မြင်စေဖို့အတွက် 
# အောက်ပါအတိုင်း အဆင့်ဆင့်သွားပါ
RUN dotnet restore "ClinicSystem.Web.csproj"
RUN dotnet publish "ClinicSystem.Web.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
