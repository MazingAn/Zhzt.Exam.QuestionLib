#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
RUN sed -i "s@http://\(deb\|security\).debian.org@https://mirrors.aliyun.com@g" /etc/apt/sources.list
RUN apt-get update -y && apt-get install -y ffmpeg && apt-get clean
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Zhzt.Exam.MicroClassLib.Api/Zhzt.Exam.MicroClassLib.Api.csproj", "Zhzt.Exam.MicroClassLib.Api/"]
RUN dotnet restore "Zhzt.Exam.MicroClassLib.Api/Zhzt.Exam.MicroClassLib.Api.csproj"
COPY . .
WORKDIR "/src/Zhzt.Exam.MicroClassLib.Api"
RUN dotnet build "Zhzt.Exam.MicroClassLib.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zhzt.Exam.MicroClassLib.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zhzt.Exam.MicroClassLib.Api.dll"]