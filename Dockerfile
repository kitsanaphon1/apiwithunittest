# ใช้ .NET 9 SDK (Preview) สร้างแอป
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /app

# ✅ คัดลอก .csproj ไปยัง build context ที่ถูกต้อง
COPY WebApp.API/WebApp.API.csproj ./WebApp.API/
RUN dotnet restore ./WebApp.API/WebApp.API.csproj

# ✅ คัดลอกไฟล์โค้ดทั้งหมด
COPY . ./

# ✅ publish โดยอ้างอิง .csproj ที่อยู่ใน WebApp.API/
RUN dotnet publish ./WebApp.API/WebApp.API.csproj -c Release -o /app/out

# ใช้ .NET 9 ASP.NET Runtime (Preview)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "WebApp.API.dll"]
