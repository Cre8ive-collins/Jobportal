FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore src/JobPortal.Api/JobPortal.Api.csproj

RUN dotnet publish src/JobPortal.Api/JobPortal.Api.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:5005

EXPOSE 5005

ENTRYPOINT ["dotnet", "JobPortal.Api.dll"]