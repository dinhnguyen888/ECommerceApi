﻿# ===========================
# STAGE 1: Build
# ===========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release --no-restore -o /out

# ===========================
# STAGE 2: Runtime 
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /out ./

EXPOSE 7202

ENTRYPOINT ["dotnet", "ECommerceApi.dll"]   