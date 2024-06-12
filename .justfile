default: build run

build:
    dotnet build WebApp.Host

run:
    dotnet run --project WebApp.Host

ef +rest:
    dotnet ef {{rest}} --project WebApp.Host

ensuredb +rest:
    just ef database drop
    just ef migrations remove
    just ef migrations add {{rest}}