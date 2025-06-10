# ใช้ .NET 9 SDK (Preview) สำหรับ build
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

# ✅ คัดลอกไฟล์โปรเจกต์
COPY WebApp.API/WebApp.API.csproj WebApp.API/
RUN dotnet restore WebApp.API/WebApp.API.csproj

# ✅ คัดลอกโค้ดทั้งหมด
COPY . .

# ✅ เปลี่ยน WORKDIR เพื่อให้ publish ทำงานใน context ของโปรเจกต์
WORKDIR /src/WebApp.API

# ✅ build และ publish
RUN dotnet publish -c Release -o /app/out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS runtime
WORKDIR /app

# ✅ ตั้งให้แอปรับ request ที่พอร์ต 80
ENV ASPNETCORE_URLS=http://+:80

# ✅ คัดลอกผลลัพธ์จาก build stage
COPY --from=build /app/out .

# ✅ รันแอป
ENTRYPOINT ["dotnet", "WebApp.API.dll"]
