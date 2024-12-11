# Use Linux-based .NET images
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ClinicalTrials.Api/ClinicalTrials.Api.csproj", "ClinicalTrials.Api/"]
COPY ["ClinicalTrials.Application/ClinicalTrials.Application.csproj", "ClinicalTrials.Application/"]
COPY ["ClinicalTrials.Domain/ClinicalTrials.Domain.csproj", "ClinicalTrials.Domain/"]
COPY ["ClinicalTrials.Infrastructure/ClinicalTrials.Infrastructure.csproj", "ClinicalTrials.Infrastructure/"]
RUN dotnet restore "./ClinicalTrials.Api/ClinicalTrials.Api.csproj"
COPY . .
WORKDIR "/src/ClinicalTrials.Api"
RUN dotnet build "./ClinicalTrials.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ClinicalTrials.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClinicalTrials.Api.dll"]