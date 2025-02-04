# Sử dụng .NET 8 SDK để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy file dự án và restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy toàn bộ source code và build
COPY . ./
RUN dotnet publish -c Release -o /out

# Image runtime chỉ chứa .NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy file đã build từ bước trước
COPY --from=build /out ./

# Expose port 7202 để truy cập từ bên ngoài
EXPOSE 7202

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "ECommerceApi.dll"]
