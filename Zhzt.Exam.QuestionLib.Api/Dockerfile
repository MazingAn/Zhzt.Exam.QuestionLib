#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
#RUN sed -i "s@http://\(deb\|security\).debian.org@https://mirrors.aliyun.com@g" /etc/apt/sources.list
#RUN apt-get update -y && apt-get install -y libgdiplus && apt-get clean && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Zhzt.Exam.QuestionLib.Api/Zhzt.Exam.QuestionLib.Api.csproj", "Zhzt.Exam.QuestionLib.Api/"]
COPY ["Zhzt.Exam.QuestionLib.DomainInterface/Zhzt.Exam.QuestionLib.DomainInterface.csproj", "Zhzt.Exam.QuestionLib.DomainInterface/"]
COPY ["SqlSugar.Extension.DomainHelper/SqlSugar.Extensions.DomainHelper.csproj", "SqlSugar.Extension.DomainHelper/"]
COPY ["Netcore.Extensions.WebModels/Netcore.Extensions.WebModels.csproj", "Netcore.Extensions.WebModels/"]
COPY ["SqlSugar.Extensions.CodeFirst/SqlSugar.Extensions.CodeFirst.csproj", "SqlSugar.Extensions.CodeFirst/"]
COPY ["Zhzt.Exam.QuestionLib.DomainModel/Zhzt.Exam.QuestionLib.DomainModel.csproj", "Zhzt.Exam.QuestionLib.DomainModel/"]
COPY ["Zhzt.Exam.QuestionLib.DomainService/Zhzt.Exam.QuestionLib.DomainService.csproj", "Zhzt.Exam.QuestionLib.DomainService/"]
COPY ["Zhzt.Exam.FileSystem/Zhzt.Exam.StaticFileSystem.csproj", "Zhzt.Exam.FileSystem/"]
RUN dotnet restore "Zhzt.Exam.QuestionLib.Api/Zhzt.Exam.QuestionLib.Api.csproj"
COPY . .
WORKDIR "/src/Zhzt.Exam.QuestionLib.Api"
RUN dotnet build "Zhzt.Exam.QuestionLib.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zhzt.Exam.QuestionLib.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zhzt.Exam.QuestionLib.Api.dll"]