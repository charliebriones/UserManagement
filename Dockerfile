# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the API project
COPY UserManagement.API/*.csproj ./UserManagement.API/
WORKDIR /src/UserManagement.API

# Restore dependencies
RUN dotnet restore

# Copy all files
COPY . .

# Publish the project
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "UserManagement.API.dll"]
