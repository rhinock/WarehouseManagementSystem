FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS build-env
WORKDIR /app
RUN mkdir WMS.UI WMS.BusinessLogic WMS.DataAccess WMS.Tests

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY WMS.UI/*.csproj ./WMS.UI/
COPY WMS.BusinessLogic/*.csproj ./WMS.BusinessLogic/
COPY WMS.DataAccess/*.csproj ./WMS.DataAccess/
COPY WMS.Tests/*.csproj ./WMS.Tests/
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "WMS.UI.dll"]
