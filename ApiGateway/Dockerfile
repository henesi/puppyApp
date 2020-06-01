FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app
COPY ./bin/Publish .
COPY Ocelot.json .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]