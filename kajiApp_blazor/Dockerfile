# ベースイメージ (ASP.NET 8.0)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# ビルド用イメージ (SDK 8.0)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# プロジェクトファイルを正しいパスでコピー
COPY ["kajiApp_blazor.csproj", "./"]

# 依存関係を復元
RUN dotnet restore "kajiApp_blazor.csproj"


# ソースコード全体をコピー
COPY . .

# ビルドの実行
RUN dotnet build "kajiApp_blazor.csproj" -c Release -o /app/build

# 公開用ステージ
FROM build AS publish
RUN dotnet publish "kajiApp_blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 実行用の最終ステージ
FROM base AS final
WORKDIR /app

# データ保護キーの永続化ディレクトリを作成
VOLUME /app/keys

# 環境変数の設定
ENV ASPNETCORE_ENVIRONMENT=Development \
    ASPNETCORE_URLS=http://0.0.0.0:8080 \
    ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    DOTNET_DATA_PROTECTION__APPLICATION_NAME=kajiApp_blazor \
    DOTNET_DATA_PROTECTION__PERSISTENT_KEYS_PATH=/app/keys

# 公開ファイルをコピー
COPY --from=publish /app/publish .

# エントリーポイント
ENTRYPOINT ["dotnet", "kajiApp_blazor.dll"]