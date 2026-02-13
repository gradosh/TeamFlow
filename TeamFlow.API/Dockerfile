# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TeamFlow.API/TeamFlow.API.csproj", "TeamFlow.API/"]
COPY ["TeamFlow.Application/TeamFlow.Application.csproj", "TeamFlow.Application/"]
COPY ["TeamFlow.Domain/TeamFlow.Domain.csproj", "TeamFlow.Domain/"]
COPY ["TeamFlow.Infrastructure/TeamFlow.Infrastructure.csproj", "TeamFlow.Infrastructure/"]

RUN dotnet restore "TeamFlow.API/TeamFlow.API.csproj"

COPY . .
WORKDIR "/src/TeamFlow.API"
RUN dotnet publish -c Release -o /app/publish

# ---------- runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "TeamFlow.API.dll"]
