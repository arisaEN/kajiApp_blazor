# ビルド環境（.NET SDK を使用）
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ソリューションファイルをコピー
COPY ["kajiApp_blazor.sln", "./"]

# プロジェクトディレクトリをコピー
COPY ["kajiApp_blazor/kajiApp_blazor.csproj", "kajiApp_blazor/"]

# 依存関係を復元
WORKDIR /src/kajiApp_blazor
RUN dotnet restore

# アプリケーションをビルド
COPY . .
RUN dotnet publish -c Release -o /app/publish

# ランタイム環境（.NET Runtime のみ）
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# アプリケーションを実行
ENTRYPOINT ["dotnet", "kajiApp_blazor.dll", "--urls=http://0.0.0.0:8080"]
