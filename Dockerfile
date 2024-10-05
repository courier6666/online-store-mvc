FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution file
COPY *.sln .  

# Copy all project files (adjust paths to match your folder structure)
COPY src/Core/*.csproj ./src/Core/
COPY src/WebApplicationMVC/*.csproj ./src/WebApplicationMVC/
COPY src/Application/*.csproj ./src/Application/
COPY src/Persistence/Main/*.csproj ./src/Persistence/Main/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/
COPY src/PersistanceTests/*.csproj ./src/PersistanceTests/

# Restore dependencies
RUN dotnet restore


# Copy everything (you can use the correct paths here)
COPY src/Core/. ./src/Core/
COPY src/WebApplicationMVC/. ./src/WebApplicationMVC/
COPY src/Application/. ./src/Application/
COPY src/Persistence/Main/. ./src/Persistence/Main/
COPY src/Infrastructure/. ./src/Infrastructure/
COPY src/PersistanceTests/. ./src/PersistanceTests/


WORKDIR /app/src/WebApplicationMVC
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 5000
COPY --from=build /app/src/WebApplicationMVC/out ./
ENTRYPOINT ["dotnet", "Store.WebApplicationMVC.dll"]
