# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Set working directory inside container
WORKDIR /src

# Copy the solution file
COPY UserManagement/UserManagement.sln ./

# Copy all project files
COPY UserManagement/UserManagement.API/*.csproj ./UserManagement.API/
COPY UserManagement/UserManagement.Application/*.csproj ./UserManagement.Application/
COPY UserManagement/UserManagement.Infrastructure/*.csproj ./UserManagement.Infrastructure/

# Restore NuGet packages
RUN dotnet restore

# Copy the rest of the source code
COPY UserManagement/UserManagement.API/. ./UserManagement.API/
COPY UserManagement/UserManagement.Application/. ./UserManagement.Application/
COPY UserManagement/UserManagement.Infrastructure/. ./UserManagement.Infrastructure/

# Build the app
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (optional, depending on your app)
EXPOSE 80
EXPOSE 443

# Run the application
ENTRYPOINT ["dotnet", "UserManagement.API.dll"]
