FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CapabilityMap.Backend.csproj ./
COPY Program.cs ./
COPY appsettings.json ./
COPY src ./src

RUN dotnet publish CapabilityMap.Backend.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:8081
EXPOSE 8081

ENTRYPOINT ["dotnet", "CapabilityMap.Backend.dll"]
