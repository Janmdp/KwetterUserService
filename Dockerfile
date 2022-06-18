#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443

#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["KwetterUserService/KwetterUserService.csproj", "KwetterUserService/"]
#RUN dotnet restore "KwetterUserService/KwetterUserService.csproj"
#COPY . .
#WORKDIR "/src/KwetterUserService"
#RUN dotnet build "KwetterUserService.csproj" -c Release -o /app/build

#FROM build AS publish
#RUN dotnet publish "KwetterUserService.csproj" -c Release -o /app/publish

#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "KwetterUserService.dll"]

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY ["KwetterUserService/KwetterUserService.csproj", "KwetterUserService/"]
COPY ["Models/Models.csproj", "KwetterUserService/"]
COPY ["Logic/Logic.csproj", "KwetterUserService/"]
COPY ["Data/Data.csproj", "KwetterUserService/"]
RUN dotnet restore "KwetterUserService/KwetterUserService.csproj"

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "KwetterUserService.dll"]