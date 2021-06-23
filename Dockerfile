#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

# To use the debug build configuration pass --build-arg Configuration=Debug
ARG Configuration=Release
ARG Runtime=linux-musl-x64
ARG Framework=netcore3.1
ARG VERSION=1.0.0
	
WORKDIR /src

COPY . .
WORKDIR "/src/TestApp.LoggingProxy.API"
RUN dotnet build "TestApp.LoggingProxy.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestApp.LoggingProxy.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestApp.LoggingProxy.API.dll"]

