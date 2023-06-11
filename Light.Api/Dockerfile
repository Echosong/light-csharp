#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Light.Api/Light.Api.csproj", "Light.Api/"]
RUN dotnet restore "Light.Api/Light.Api.csproj"
COPY . .
WORKDIR "/src/Light.Api"
RUN dotnet build "Light.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Light.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Light.Api.dll"]


# /home/song/wwwroot/publish

# dotnet,Light.Api.dll

# sudo docker cp b658712c31f1a943f77c6b4bbdd87742ee973564533fd525e6c02a167453b599:/etc/ssl/openssl.cnf /home/song/wwwroot/
# sudo docker cp /home/song/wwwroot/openssl.cnf  b658712c31f1a943f77c6b4bbdd87742ee973564533fd525e6c02a167453b599:/etc/ssl/
