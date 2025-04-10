FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /app

COPY ../Directory.Packages.props ./
COPY ../WebApp ./WebApp
COPY ../WebApp.Api.V1 ./WebApp.Api.V1

WORKDIR /app/WebApp.Api.V1
RUN dotnet publish -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app/WebApp.Api.V1
COPY --from=build /app/WebApp.Api.V1/out ./

EXPOSE 5051

ENTRYPOINT ["dotnet", "WebApp.Api.V1.dll"]
