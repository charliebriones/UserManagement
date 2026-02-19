# -----------------------------
# Build Stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the solution file
COPY UserManagement/UserManagement.sln ./

# Copy project files
COPY UserManagement/UserManagement.API/*.csproj ./UserManagement.API/
COPY UserManagement/UserManagement.Application/*.csproj ./UserManagement.Application/
COPY UserManagement/UserManagement.Infrastructure/*.csproj ./UserManagement.Infrastructure/

# Restore dependencies
RUN dotnet restore

# Copy the remaining source code
COPY UserManagement/UserManagement.API/. ./UserManagement.API/
COPY UserManagement/UserManagement.Application/. ./UserManagement.Application/
COPY UserManagement/UserManagement.Infrastructure/. ./UserManagement.Infrastructure/

# Build the app
WORKDIR /src/UserManagement.API
RUN dotnet publish -c Release -o /app/publish

# -----------------------------
# Runtime Stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy the published output from build stage
COPY --from=build /app/publish ./

# Expose the port your app listens on
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "UserManagement.API.dll"]
