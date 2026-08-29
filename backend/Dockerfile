# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/ExampleCom.ProductManagement.Api/*.csproj ./ExampleCom.ProductManagement.Api/
RUN dotnet restore ExampleCom.ProductManagement.Api/ExampleCom.ProductManagement.Api.csproj
COPY src/ .
WORKDIR /src/ExampleCom.ProductManagement.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "ExampleCom.ProductManagement.Api.dll"]
