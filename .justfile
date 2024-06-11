default: run

build:
    dotnet build WebApp.Host

run:
    dotnet run --project WebApp.Host

ef +rest:
    dotnet ef {{rest}} --project WebApp.Host