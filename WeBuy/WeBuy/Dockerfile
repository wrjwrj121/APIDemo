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


#���뾵�񣬵Ͱ汾 docker ȥ��  AS base
#���ɰ��ﾵ��
#FROM registry.cn-chengdu.aliyuncs.com/jimlicatpub/aspnet:3.1-buster-slim AS base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
 
#���ù���Ŀ¼
WORKDIR /app
  
#��¶�����˿ڣ��˶˿����������·��һ�£���
EXPOSE 80
  
#�����ļ�������Ŀ¼
COPY . .
 
#ENV ������ϵͳ����������������򻷾�����������������(������Ԥ�������ϻ���)
#���������ó������ж˿� �������ʹ��Ĭ�ϵ�80�˿�����һ��Ҫ���ã��������ж˿ڣ�
ENV ASPNETCORE_URLS http://+:80
 
#��������
ENTRYPOINT ["dotnet", "WeBuy.dll"]