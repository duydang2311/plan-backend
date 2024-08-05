set dotenv-load

default: build run

build:
    dotnet build WebApp.AppHost

run:
    dotnet run --project WebApp.AppHost

ef +rest:
    dotnet ef {{rest}} --project WebApp.Api.V1 --context AppDbContext

ensuredb +rest:
    just ef database drop
    just ef migrations remove
    just ef migrations add {{rest}}
