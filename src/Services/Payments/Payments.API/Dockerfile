FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["MercuryPlatform.sln", "./"]

# Copy all project files
COPY ["src/Services/Payments/Payments.API/Payments.API.csproj", "src/Services/Payments/Payments.API/"]
COPY ["src/Services/Payments/Payments.Application/Payments.Application.csproj", "src/Services/Payments/Payments.Application/"]
COPY ["src/Services/Payments/Payments.Domain/Payments.Domain.csproj", "src/Services/Payments/Payments.Domain/"]
COPY ["src/Services/Payments/Payments.Infrastructure/Payments.Infrastructure.csproj", "src/Services/Payments/Payments.Infrastructure/"]
COPY ["src/Shared/Shared.Contracts/Shared.Contracts.csproj", "src/Shared/Shared.Contracts/"]
COPY ["src/Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "src/Shared/Shared.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/Services/Payments/Payments.API/Payments.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/src/Services/Payments/Payments.API"
RUN dotnet build "Payments.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payments.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payments.API.dll"]
