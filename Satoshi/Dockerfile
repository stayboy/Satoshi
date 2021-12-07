FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

LABEL author="Solomon"

ENV ASPNETCORE_ENVIRONMENT=development
ENV PORT=5000

WORKDIR /app
EXPOSE $PORT
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Satoshi/Satoshi.csproj", "Satoshi/"]
RUN dotnet restore "Satoshi/Satoshi.csproj"
COPY . .
WORKDIR "/src/Satoshi"
RUN dotnet build "Satoshi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Satoshi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Satoshi.dll"]