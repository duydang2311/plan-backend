default: build run

build:
    dotnet build WebApp.Api.V1

run:
    dotnet run --project WebApp.Api.V1

ef +rest:
    dotnet ef {{rest}} --project WebApp.Api.V1 --context AppDbContext

ensuredb +rest:
    just ef database drop
    just ef migrations remove
    just ef migrations add {{rest}}