# Etapa 1: compilar y publicar
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY DSW_3P_PR01.csproj ./
RUN dotnet restore DSW_3P_PR01.csproj

COPY . ./
RUN dotnet publish DSW_3P_PR01.csproj -c Release -o /app/publish --no-restore

# Etapa 2: imagen final solo con el runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_ENVIRONMENT=Production

# Render indica el puerto en la variable PORT; localmente se usa 8080.
EXPOSE 8080
ENTRYPOINT ["sh", "-c", "ASPNETCORE_HTTP_PORTS=${PORT:-8080} exec dotnet DSW_3P_PR01.dll"]
