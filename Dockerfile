FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Tiktack.Messages.WebApi/Tiktack.Messaging.WebApi.csproj", "Tiktack.Messages.WebApi/"]
COPY ["Tiktack.Messaging.DataAccessLayer/Tiktack.Messaging.DataAccessLayer.csproj", "Tiktack.Messaging.DataAccessLayer/"]
COPY ["Tiktack.Common.DataAccessLayer/Tiktack.Common.DataAccessLayer.csproj", "Tiktack.Common.DataAccessLayer/"]
COPY ["Tiktack.Messaging.BusinessLayer/Tiktack.Messaging.BusinessLayer.csproj", "Tiktack.Messaging.BusinessLayer/"]
RUN dotnet restore "Tiktack.Messages.WebApi/Tiktack.Messaging.WebApi.csproj"
COPY . .
WORKDIR "/src/Tiktack.Messages.WebApi"
RUN dotnet build "Tiktack.Messaging.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Tiktack.Messaging.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Tiktack.Messaging.WebApi.dll"]