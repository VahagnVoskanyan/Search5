FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Search_Service.csproj", "."]
RUN dotnet restore "./Search_Service.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Search_Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Search_Service.csproj" -c Release -o /app/publish #/p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Search_Service.dll"]