# .NET SDK kullanılarak uygulama build edilecek
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Proje dosyalarını kopyala ve restore et
COPY ./EnviromentSystem.sln ./
COPY ./API/API.csproj ./API/
COPY ./Core/Core.csproj ./Core/
RUN dotnet restore EnviromentSystem.sln

# Tüm projeleri kopyala ve build et
COPY . .
WORKDIR /app/API
RUN dotnet publish API.csproj -c Release -o /publish

# Uygulama için runtime image kullan
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /publish .

# API projesi için giriş noktası
ENTRYPOINT ["dotnet", "API.dll"]

# Port bilgisi
EXPOSE 5154
