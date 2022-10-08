#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
RUN sed -i "s@http://\(deb\|security\).debian.org@https://mirrors.aliyun.com@g" /etc/apt/sources.list
RUN apt-get update -y && apt-get install -y apt-utils
RUN apt-get install -y libgdiplus && apt-get install -y libc6-dev
RUN ln -s /usr/bin/libgdiplus.so /usr/bin/gdiplus.dll
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Zhzt.Exam.DocumentLib.Api/Zhzt.Exam.DocumentLib.Api.csproj", "Zhzt.Exam.DocumentLib.Api/"]
COPY ["MicroService.NacosDiscover.Helper/MicroService.NacosDiscover.Helper.csproj", "MicroService.NacosDiscover.Helper/"]
COPY ["SqlSugar.Extensions.CodeFirst/SqlSugar.Extensions.CodeFirst.csproj", "SqlSugar.Extensions.CodeFirst/"]
COPY ["Zhzt.Exam.Document.DomainService/Zhzt.Exam.Document.DomainService.csproj", "Zhzt.Exam.Document.DomainService/"]
COPY ["Zhzt.Exam.Document.DomainInterface/Zhzt.Exam.Document.DomainInterface.csproj", "Zhzt.Exam.Document.DomainInterface/"]
COPY ["Zhzt.Exam.Document.DomainModel/Zhzt.Exam.Document.DomainModel.csproj", "Zhzt.Exam.Document.DomainModel/"]
COPY ["SqlSugar.Extension.DomainHelper/SqlSugar.Extensions.DomainHelper.csproj", "SqlSugar.Extension.DomainHelper/"]
COPY ["Netcore.Extensions.WebModels/Netcore.Extensions.WebModels.csproj", "Netcore.Extensions.WebModels/"]
COPY ["Zhzt.Exam.FileSystem/Zhzt.Exam.StaticFileSystem.csproj", "Zhzt.Exam.FileSystem/"]
RUN dotnet restore "Zhzt.Exam.DocumentLib.Api/Zhzt.Exam.DocumentLib.Api.csproj"
COPY . .
WORKDIR "/src/Zhzt.Exam.DocumentLib.Api"
RUN dotnet build "Zhzt.Exam.DocumentLib.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zhzt.Exam.DocumentLib.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /usr/share/fonts/ttf-dejavu/ ./fonts/*
ENTRYPOINT ["dotnet", "Zhzt.Exam.DocumentLib.Api.dll"]