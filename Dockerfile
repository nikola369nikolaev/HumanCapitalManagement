FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HumanCapitalManagement/HumanCapitalManagement.csproj", "HumanCapitalManagement/"]
RUN dotnet restore "HumanCapitalManagement/HumanCapitalManagement.csproj"
COPY . .
WORKDIR "/src/HumanCapitalManagement"
RUN dotnet build "HumanCapitalManagement.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HumanCapitalManagement.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "HumanCapitalManagement.dll"]
