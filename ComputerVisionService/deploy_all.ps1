dotnet publish -p:PublishProfile=Properties\PublishProfiles\Publish.pubxml
docker build -t computer-vision -f Dockerfile .