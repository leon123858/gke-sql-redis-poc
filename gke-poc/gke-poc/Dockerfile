FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["gke-poc/gke-poc.csproj", "gke-poc/"]
RUN dotnet restore "gke-poc/gke-poc.csproj"
COPY . .
WORKDIR "/src/gke-poc"
RUN dotnet build "gke-poc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gke-poc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gke-poc.dll"]
