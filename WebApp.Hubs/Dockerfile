FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

COPY ../Directory.Packages.props ./
COPY ../WebApp ./WebApp
COPY ../WebApp.Hubs ./WebApp.Hubs
RUN dotnet restore WebApp.Hubs

WORKDIR /app/WebApp.Hubs
RUN dotnet publish -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app/WebApp.Hubs
COPY --from=build /app/WebApp.Hubs/out ./

EXPOSE 5051

ENTRYPOINT ["dotnet", "WebApp.Hubs.dll"]
