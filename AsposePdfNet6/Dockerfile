#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AsposePdfNet6/AsposePdfNet6.csproj", "AsposePdfNet6/"]
RUN dotnet restore "AsposePdfNet6/AsposePdfNet6.csproj"
COPY . .
WORKDIR "/src/AsposePdfNet6"
RUN dotnet build "AsposePdfNet6.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AsposePdfNet6.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN apt-get update
RUN apt-get install -y libfontconfig1 
RUN apt-get install -y libgdiplus 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AsposePdfNet6.dll"]