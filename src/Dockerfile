FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["VectorSearch.API/VectorSearch.API.csproj", "VectorSearch.API/"]
COPY ["VectorSearch.Application/VectorSearch.Application.csproj", "VectorSearch.Application/"]
COPY ["VectorSearch.Domain/VectorSearch.Domain.csproj", "VectorSearch.Domain/"]
COPY ["VectorSearch.Infrastructure/VectorSearch.Infrastructure.csproj", "VectorSearch.Infrastructure/"]

RUN dotnet restore "VectorSearch.API/VectorSearch.API.csproj"

COPY . .

RUN dotnet publish "VectorSearch.API/VectorSearch.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VectorSearch.API.dll"]