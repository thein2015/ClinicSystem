# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# ဖိုင်များအားလုံးကို ကူးယူပါ
COPY . .

# Shared ပရောဂျက်ကို အရင် Restore လုပ်ပါ
# သင့် Repository တွင် ClinicSystem.Shared folder မရှိပါက ဤလိုင်းကို ဖျက်ပါ
RUN dotnet restore "ClinicSystem.Shared/ClinicSystem.Shared.csproj" || true

# Web ပရောဂျက်ကို Restore နှင့် Publish လုပ်ပါ
RUN dotnet restore "ClinicSystem.Web.csproj"
RUN dotnet publish "ClinicSystem.Web.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClinicSystem.Web.dll"]
