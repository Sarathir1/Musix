#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Musix.Api/Musix.Api.csproj", "Musix.Api/"]
RUN dotnet restore "Musix.Api/Musix.Api.csproj"
COPY . .
WORKDIR "/src/Musix.Api"
RUN dotnet build "Musix.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Musix.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Musix.Api.dll"]