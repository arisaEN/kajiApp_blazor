# デバッグ コンテナーをカスタマイズする方法については、以下を参照してください。
# https://aka.ms/customizecontainer

# (1) 実行環境のベースイメージを指定
# ASP.NET Core ランタイムが含まれており、アプリを実行するためのベースとなる
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app  
# コンテナ内部でポート 2001 を開放する
EXPOSE 3001

# (2) ASP.NET Core の実行ポートを設定
# コンテナ内で ASP.NET Core アプリがポート 2001 でリッスンするようにする
ENV ASPNETCORE_URLS="http://+:3001"

# (3) ビルド環境のベースイメージを指定
# .NET SDK を含むイメージを使用し、アプリをビルドする
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release  
WORKDIR /src  

# (4) 依存関係の復元 (NuGet パッケージの取得)
# プロジェクトファイル (.csproj) のみをコピーし、`dotnet restore` を実行
# これにより、キャッシュを利用してビルド時間を短縮できる
COPY ["kajiApp_blazor/kajiApp_blazor.csproj", "kajiApp_blazor/"]
RUN dotnet restore "./kajiApp_blazor/kajiApp_blazor.csproj"

# (5) アプリケーションのソースコードをコピー
# プロジェクトのすべてのファイルをコンテナ内にコピーする
COPY . .  
WORKDIR "/src/kajiApp_blazor"  

# (6) アプリケーションをビルド
# `dotnet build` を実行し、ビルド成果物を `/app/build` に出力する
RUN dotnet build "./kajiApp_blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

# (7) アプリケーションの公開
# `dotnet publish` を実行し、実行可能な形式で `/app/publish` に出力する
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./kajiApp_blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# (8) 本番環境のコンテナを作成
# ビルド済みのアプリを `base` イメージにコピーし、実
FROM base AS final
WORKDIR /app  
COPY --from=publish /app/publish .  

# (9) アプリケーションを起動
# `dotnet kajiApp_blazor.dll` を実行し、アプリケーションを起動する
ENTRYPOINT ["dotnet", "kajiApp_blazor.dll"]
