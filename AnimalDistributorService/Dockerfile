FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app
COPY ./bin/Publish .
COPY ./Media ./Media
ENTRYPOINT ["dotnet", "AnimalDistributorService.dll"]