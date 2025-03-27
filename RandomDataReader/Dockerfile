# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy just the project file and restore
COPY ["RandomDataReader.csproj", "./"]
RUN dotnet restore "RandomDataReader.csproj"

# Copy remaining source code
COPY . .
RUN dotnet build "RandomDataReader.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "RandomDataReader.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create directories for mounted volumes
RUN mkdir -p /data/input && mkdir -p /data/output

# Copy published app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "RandomDataReader.dll"]