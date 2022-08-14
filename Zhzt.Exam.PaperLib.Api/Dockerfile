#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Zhzt.Exam.PaperLib.Api/Zhzt.Exam.PaperLib.Api.csproj", "Zhzt.Exam.PaperLib.Api/"]
COPY ["Zhzt.Exam.PagerLib.DomainModel/Zhzt.Exam.PaperLib.DomainModel.csproj", "Zhzt.Exam.PagerLib.DomainModel/"]
COPY ["MongoDb.Extensions.DomainHelper/MongoDb.Extensions.DomainHelper.csproj", "MongoDb.Extensions.DomainHelper/"]
COPY ["MongoDb.Extensions.Options/MongoDb.Extensions.Options.csproj", "MongoDb.Extensions.Options/"]
COPY ["Netcore.Extensions.WebModels/Netcore.Extensions.WebModels.csproj", "Netcore.Extensions.WebModels/"]
COPY ["Zhzt.Exam.PaperLib.DomainInterface/Zhzt.Exam.PaperLib.DomainInterface.csproj", "Zhzt.Exam.PaperLib.DomainInterface/"]
COPY ["Zhzt.Exam.PaperLib.DomainService/Zhzt.Exam.PaperLib.DomainService.csproj", "Zhzt.Exam.PaperLib.DomainService/"]
RUN dotnet restore "Zhzt.Exam.PaperLib.Api/Zhzt.Exam.PaperLib.Api.csproj"
COPY . .
WORKDIR "/src/Zhzt.Exam.PaperLib.Api"
RUN dotnet build "Zhzt.Exam.PaperLib.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zhzt.Exam.PaperLib.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zhzt.Exam.PaperLib.Api.dll"]