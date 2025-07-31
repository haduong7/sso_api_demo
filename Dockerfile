# Use .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy source code and build
COPY . ./
RUN dotnet publish -c Release -o out

# Use .NET 8 runtime for final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy built application
COPY --from=build /app/out .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port (Railway uses 8080 by default)
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "SSO_Api.dll"]