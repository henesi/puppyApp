FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app
COPY ./bin/Publish .
ENTRYPOINT ["dotnet", "AnimalSearchService.dll"]