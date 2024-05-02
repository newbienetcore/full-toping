FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/Services/Catalog/Catalog.Api/Catalog.Api.csproj", "Src/Services/Catalog/Catalog.Api/"]
COPY ["Src/BuildingBlocks/Logging/Logging.csproj", "Src/BuildingBlocks/Logging/"]
COPY ["Src/BuildingBlocks/SharedKernel/SharedKernel.csproj", "Src/BuildingBlocks/SharedKernel/"]
COPY ["Src/BuildingBlocks/Caching/Caching.csproj", "Src/BuildingBlocks/Caching/"]
RUN dotnet restore "Src/Services/Catalog/Catalog.Api/Catalog.Api.csproj"
COPY . .
WORKDIR "/src/Src/Services/Catalog/Catalog.Api"
RUN dotnet build "Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]
