FROM microsoft/aspnetcore-build:1.1

WORKDIR /app

COPY . .

RUN dotnet restore
ENTRYPOINT ["dotnet","run"]
