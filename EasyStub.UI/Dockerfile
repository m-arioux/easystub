FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY EasyStub.UI.csproj .
RUN dotnet restore EasyStub.UI.csproj
COPY . .
RUN dotnet build EasyStub.UI.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish EasyStub.UI.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf