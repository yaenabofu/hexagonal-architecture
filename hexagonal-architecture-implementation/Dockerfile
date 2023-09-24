FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Adapter.RestApi/Adapter.RestApi.csproj", "Adapter.RestApi/"]
COPY ["Application.RestApi/Application.RestApi.csproj", "Application.RestApi/"]
COPY ["Adapter.HMACSHA256-PasswordHasher/Adapter.HMACSHA256-PasswordHasher.csproj", "Adapter.HMACSHA256-PasswordHasher/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Adapter.MsSqlServer/Adapter.MsSqlServer.csproj", "Adapter.MsSqlServer/"]
RUN dotnet restore "Adapter.RestApi/Adapter.RestApi.csproj"
COPY . .
WORKDIR "/src/Adapter.RestApi"
RUN dotnet build "Adapter.RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adapter.RestApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adapter.RestApi.dll"]