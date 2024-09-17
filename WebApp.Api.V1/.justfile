set dotenv-load

default: build run

build:
    dotnet build

run:
    dotnet run

ef +rest:
    dotnet ef {{rest}} --context AppDbContext

ensuredb +rest:
    just ef database drop
    just ef migrations remove
    just ef migrations add {{rest}}
