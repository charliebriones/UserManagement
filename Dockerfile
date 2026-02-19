FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the solution file
COPY UserManagement.sln ./

# Copy all projects
COPY UserManagement.API/*.csproj ./UserManagement.API/
COPY UserManagement.Application/*.csproj ./UserManagement.Application/
COPY UserManagement.Infrastructure/*.csproj ./UserManagement.Infrastructure/

# Restore solution
RUN dotnet restore

# Copy all source files
COPY . .

# Build
RUN dotnet build -c Release --no-restore

# Publish
RUN dotnet publish UserManagement.API/UserManagement.API.csproj -c Release -o /app --no-build

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "UserManagement.API.dll"]
