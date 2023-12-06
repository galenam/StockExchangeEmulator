
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS publish
WORKDIR /src

COPY . ./

RUN dotnet publish "Src/StockExchangeEmulator.Emulator/StockExchangeEmulator.Emulator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockExchangeEmulator.Emulator.dll"]
