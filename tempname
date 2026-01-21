# Use official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies first (cache layer)
COPY *.sln .
COPY mymusic_app/*.csproj ./mymusic_app/
RUN dotnet restore

# Copy the rest of the source code
COPY mymusic_app/. ./mymusic_app/

# Build the project
WORKDIR /src/mymusic_app
RUN dotnet publish -c Release -o /app/publish

# Use the official runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80 (or 5000 if you prefer)
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "mymusic_app.dll"]