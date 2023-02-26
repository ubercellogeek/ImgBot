FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /build

ARG buildConfig=Release

COPY . ./
RUN dotnet publish ImgBot.Api.csproj -c ${buildConfig} -o out -p:DebugType=full -p:DebugSymbols=true

FROM base AS final
WORKDIR /app
COPY --from=build /build/out .
ENTRYPOINT ["dotnet", "ImgBot.Api.dll"]