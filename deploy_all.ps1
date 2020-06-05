dotnet build AnimalDistributorService\AnimalDistributorService.csproj /p:DeployOnBuild=true /p:PublishProfile=AnimalDistributorService\Properties\PublishProfiles\Publish.pubxml
docker build -t puppy-distribution -f AnimalDistributorService\Dockerfile .

dotnet build AnimalSearchService\AnimalSearchService.csproj /p:DeployOnBuild=true /p:PublishProfile=AnimalSearchService\Properties\PublishProfiles\Publish.pubxml
docker build -t puppy-search -f AnimalSearchService\Dockerfile .

dotnet build ApiGateway\ApiGateway.csproj /p:DeployOnBuild=true /p:PublishProfile=ApiGateway\Properties\PublishProfiles\Publish.pubxml
docker build -t puppy-apigateway -f ApiGateway\Dockerfile .

dotnet build AuthService\AuthService.csproj /p:DeployOnBuild=true /p:PublishProfile=AuthService\Properties\PublishProfiles\Publish.pubxml
docker build -t puppy-auth -f AuthService\Dockerfile .

dotnet build ComputerVisionService\ComputerVisionService.csproj /p:DeployOnBuild=true /p:PublishProfile=ComputerVisionService\Properties\PublishProfiles\Publish.pubxml
docker build -t computer-vision -f ComputerVisionService\Dockerfile .