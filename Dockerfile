# Use official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy solution file first
COPY *.sln ./

# Copy csproj files for restore
COPY mymusic-app/*.cs ./mymusic-app/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY mymusic-app/. ./mymusic-app/

# Build the project
WORKDIR /src/mymusic-app
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "mymusic-app.dll"]
