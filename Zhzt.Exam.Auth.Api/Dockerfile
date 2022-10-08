#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Zhzt.Exam.Auth.Api/Zhzt.Exam.Auth.Api.csproj", "Zhzt.Exam.Auth.Api/"]
COPY ["MicroService.NacosDiscover.Helper/MicroService.NacosDiscover.Helper.csproj", "MicroService.NacosDiscover.Helper/"]
COPY ["Netcore.Extensions.WebModels/Netcore.Extensions.WebModels.csproj", "Netcore.Extensions.WebModels/"]
COPY ["SqlSugar.Extension.DomainHelper/SqlSugar.Extensions.DomainHelper.csproj", "SqlSugar.Extension.DomainHelper/"]
COPY ["SqlSugar.Extensions.CodeFirst/SqlSugar.Extensions.CodeFirst.csproj", "SqlSugar.Extensions.CodeFirst/"]
COPY ["Zhzt.Exam.User.DomainService/Zhzt.Exam.Auth.DomainService.csproj", "Zhzt.Exam.User.DomainService/"]
COPY ["Zhzt.Exam.Auth.DtoHelper/Zhzt.Exam.Auth.DomainDtoHelper.csproj", "Zhzt.Exam.Auth.DtoHelper/"]
COPY ["Zhzt.Exam.User.DomainModel/Zhzt.Exam.Auth.DomainModel.csproj", "Zhzt.Exam.User.DomainModel/"]
COPY ["Amz.EnumLib/Amz.EnumLib.csproj", "Amz.EnumLib/"]
COPY ["Zhzt.Exam.Auth.DomainDtoModel/Zhzt.Exam.Auth.DomainDtoModel.csproj", "Zhzt.Exam.Auth.DomainDtoModel/"]
COPY ["Zhzt.Exam.User.Interface/Zhzt.Exam.Auth.DomainInterface.csproj", "Zhzt.Exam.User.Interface/"]
RUN dotnet restore "Zhzt.Exam.Auth.Api/Zhzt.Exam.Auth.Api.csproj"
COPY . .
WORKDIR "/src/Zhzt.Exam.Auth.Api"
RUN dotnet build "Zhzt.Exam.Auth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zhzt.Exam.Auth.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zhzt.Exam.Auth.Api.dll"]