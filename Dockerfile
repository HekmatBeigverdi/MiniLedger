FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["MiniLedger.sln", "."]
COPY ["src/MiniLedger.Api/MiniLedger.Api.csproj", "src/MiniLedger.Api/"]
COPY ["src/MiniLedger.Application/MiniLedger.Application.csproj", "src/MiniLedger.Application/"]
COPY ["src/MiniLedger.Domain/MiniLedger.Domain.csproj", "src/MiniLedger.Domain/"]
COPY ["src/MiniLedger.Infrastructure/MiniLedger.Infrastructure.csproj", "src/MiniLedger.Infrastructure/"]

RUN dotnet restore "MiniLedger.sln"

COPY . .
RUN dotnet publish "src/MiniLedger.Api/MiniLedger.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "MiniLedger.Api.dll"]
