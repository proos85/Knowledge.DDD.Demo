FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /app

# Copy csproj and restore
COPY ./src .
RUN dotnet restore

# Copy everything else and build
RUN dotnet publish -c Release -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS Run
WORKDIR /app
COPY --from=build /dist /app
ENTRYPOINT ["dotnet", "Knowledge.DDD.Demo.WebApi.dll"]