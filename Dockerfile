FROM microsoft/aspnetcore-build:2.0 as build-env
WORKDIR /app


#COPY *.csproj ./
#RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/out

FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CoreDMS.dll"]