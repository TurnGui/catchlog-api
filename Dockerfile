# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CatchLog.Api/CatchLog.Api.csproj CatchLog.Api/
RUN dotnet restore CatchLog.Api/CatchLog.Api.csproj

COPY CatchLog.Api/ CatchLog.Api/
RUN dotnet publish CatchLog.Api/CatchLog.Api.csproj -c Release -o /app/publish --no-restore

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CatchLog.Api.dll"]