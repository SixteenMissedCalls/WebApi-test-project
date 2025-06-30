# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CurrencyRateGateway/CurrencyRateGateway.csproj", "CurrencyRateGateway/"]
COPY ["CurrencyRateGateway.Application/CurrencyRateGateway.Application.csproj", "CurrencyRateGateway.Application/"]
COPY ["CurrencyRateGateway.Entities/CurrencyRateGateway.Entities.csproj", "CurrencyRateGateway.Entities/"]
COPY ["CurrencyRateGateway.Web/CurrencyRateGateway.Web.csproj", "CurrencyRateGateway.Web/"]
RUN dotnet restore "./CurrencyRateGateway/CurrencyRateGateway.csproj"
COPY . .
WORKDIR "/src/CurrencyRateGateway"
RUN dotnet build "./CurrencyRateGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CurrencyRateGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyRateGateway.dll"]