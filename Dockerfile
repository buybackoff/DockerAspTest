FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY DockerAspTest.csproj DockerAspTest/
RUN dotnet restore DockerAspTest/DockerAspTest.csproj
COPY . ./DockerAspTest/
WORKDIR /src/DockerAspTest
RUN dotnet build DockerAspTest.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DockerAspTest.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DockerAspTest.dll"]
