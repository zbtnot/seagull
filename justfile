build:
    dotnet build
test:
    dotnet test

# helper commands for testing produced binaries
init-project: clean-project
    dotnet run --project ./src new /tmp/derp

build-project:
    dotnet run --project ./src build /tmp/derp /tmp/out

clean-project:
    rm -rf /tmp/derp
