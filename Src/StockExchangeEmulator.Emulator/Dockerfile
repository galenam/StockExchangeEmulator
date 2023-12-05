
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Src/StockExchangeEmulator.Domain/StockExchangeEmulator.Domain.csproj", "src/StockExchangeEmulator.Domain/"]
COPY ["Src/StockExchangeEmulator.Emulator/StockExchangeEmulator.Emulator.csproj", "src/StockExchangeEmulator.Emulator/"]
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Directory.Build.targets", "Directory.Build.targets"]
COPY ["Directory.Packages.props", "Directory.Packages.props"]

RUN dotnet restore "src/StockExchangeEmulator.Emulator/StockExchangeEmulator.Emulator.csproj"
COPY . .
WORKDIR "src/StockExchangeEmulator.Emulator"

RUN dotnet build "StockExchangeEmulator.Emulator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockExchangeEmulator.Emulator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockExchangeEmulator.Emulator.dll"]
