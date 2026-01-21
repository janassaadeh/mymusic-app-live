# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln ./
COPY mymusic-app/*.csproj ./mymusic-app/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY mymusic-app/. ./mymusic-app/

# Publish the app
WORKDIR /src/mymusic-app
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 80

# Start app
ENTRYPOINT ["dotnet", "mymusic-app.dll"]
