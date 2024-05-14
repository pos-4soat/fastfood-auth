FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

RUN dotnet restore "src/fastfood-auth.API/fastfood-auth.API.csproj"
RUN dotnet build "src/fastfood-auth.API/fastfood-auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/fastfood-auth.API/fastfood-auth.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "fastfood-products.API.dll"]