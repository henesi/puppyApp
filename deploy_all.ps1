$hash =
@{ 
	AnimalDistributorService = "puppy-distribution"; 
	AnimalSearchService = "puppy-search"; 
	ApiGateway = "puppy-apigateway";
	AuthService = "puppy-auth";
	ComputerVisionService = "computer-vision";
}

#Dotnet
foreach ($h in $hash.Keys) {
	# temporarily change to the correct folder
	$folder = Resolve-Path ".\${h}\"
	Push-Location $folder
	
	#build/publish
	dotnet build "${h}.csproj" /p:DeployOnBuild=true /p:PublishProfile=Properties\PublishProfiles\Publish.pubxml
	# do stuff, call ant, etc
	docker build -t $($hash.Item($h)) -f Dockerfile .

	# now back to previous directory
	Pop-Location
}

#UI
$folder = Resolve-Path ".\UI\"
Push-Location $folder

# do stuff, call ant, etc
docker build -t $ "puppy-ui" -f Dockerfile .

# now back to previous directory
Pop-Location