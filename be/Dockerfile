FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 82

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Calca/Calca.csproj", "Calca/"]
RUN dotnet restore "Calca/Calca.csproj"
COPY . .
WORKDIR "/src/Calca"
RUN dotnet build "Calca.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Calca.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Calca.dll"]