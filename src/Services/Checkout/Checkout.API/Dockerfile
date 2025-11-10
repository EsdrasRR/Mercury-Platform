FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["MercuryPlatform.sln", "./"]

# Copy all project files
COPY ["src/Services/Checkout/Checkout.API/Checkout.API.csproj", "src/Services/Checkout/Checkout.API/"]
COPY ["src/Services/Checkout/Checkout.Application/Checkout.Application.csproj", "src/Services/Checkout/Checkout.Application/"]
COPY ["src/Services/Checkout/Checkout.Domain/Checkout.Domain.csproj", "src/Services/Checkout/Checkout.Domain/"]
COPY ["src/Services/Checkout/Checkout.Infrastructure/Checkout.Infrastructure.csproj", "src/Services/Checkout/Checkout.Infrastructure/"]
COPY ["src/Shared/Shared.Contracts/Shared.Contracts.csproj", "src/Shared/Shared.Contracts/"]
COPY ["src/Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "src/Shared/Shared.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/Services/Checkout/Checkout.API/Checkout.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/src/Services/Checkout/Checkout.API"
RUN dotnet build "Checkout.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.API.dll"]
