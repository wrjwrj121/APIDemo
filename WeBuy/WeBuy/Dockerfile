#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat
#
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
#WORKDIR /src
#COPY ["WeBuy/WeBuy.csproj", "WeBuy/"]
#COPY ["WeBuy.IService/WeBuy.IService.csproj", "WeBuy.IService/"]
#COPY ["WeBuyModel/WeBuy.Model.csproj", "WeBuyModel/"]
#COPY ["WeBuy.Service/WeBuy.Service.csproj", "WeBuy.Service/"]
#COPY ["WeBuy.Common/WeBuy.Common.csproj", "WeBuy.Common/"]
#RUN dotnet restore "WeBuy/WeBuy.csproj"
#COPY . .
#WORKDIR "/src/WeBuy"
#RUN dotnet build "WeBuy.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "WeBuy.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "WeBuy.dll"]


#引入镜像，低版本 docker 去掉  AS base
#换成阿里镜像
#FROM registry.cn-chengdu.aliyuncs.com/jimlicatpub/aspnet:3.1-buster-slim AS base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
 
#配置工作目录
WORKDIR /app
  
#暴露容器端口，此端口与程序运行路径一致，可
EXPOSE 80
  
#复制文件到工作目录
COPY . .
 
#ENV ：配置系统环境变量，比如程序环境环境等在这里配置(开发、预发、线上环境)
#这里是配置程序运行端口 如果程序不使用默认的80端口这里一定要设置（程序运行端口）
ENV ASPNETCORE_URLS http://+:80
 
#启动程序
ENTRYPOINT ["dotnet", "WeBuy.dll"]