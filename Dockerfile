# ----------------------------
# Stage 1: Build
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file first
COPY *.sln ./

# Copy csproj files
COPY mymusic-app/*.csproj ./mymusic-app/

# Restore dependencies using the solution file
RUN dotnet restore *.sln

# Copy the rest of the source code
COPY mymusic-app/. ./mymusic-app/

# Build & publish the app
WORKDIR /src/mymusic-app
RUN dotnet publish -c Release -o /app/publish

# ----------------------------
# Stage 2: Runtime
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "mymusic-app.dll"]
