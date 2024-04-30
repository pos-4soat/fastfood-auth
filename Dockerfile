FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

RUN dotnet publish -c Release src/fastfood-auth.csproj -o /publish

FROM base AS final
WORKDIR /app
COPY --from=build /publish ./

ENTRYPOINT ["dotnet", "fastfood-auth.dll"]