# Use the official .NET 6 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy .csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application and build
COPY . ./
RUN dotnet publish -c Release -o out

# Use a lightweight .NET 6 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

# Set environment variables for the app
ENV ASPNETCORE_URLS=http://+:7202

# Expose port 5000
EXPOSE 5000

# Run the application
ENTRYPOINT ["dotnet", "ECommerceApi.dll"]
