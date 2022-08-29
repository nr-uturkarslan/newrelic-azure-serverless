#!/bin/bash

#############
### Setup ###
#############

### Set parameters
project="nr1"
locationLong="westeurope"
locationShort="euw"
stageLong="dev"
stageShort="d"
instance="001"

app="archive"

### Set variables
resourceGroupName="rg${project}${locationShort}${platform}${stageShort}${instance}"
appServiceName="as${project}${locationShort}${app}${stageShort}${instance}"

###########
### App ###
###########

# Publish code
dotnet publish \
  ../../apps/ArchiveService/ArchiveService/ArchiveService.csproj \
  -c Release

# Zip binaries
currentDir=$(pwd)
cd ../../apps/ArchiveService/ArchiveService/bin/Release/net6.0/publish
zip -r publish.zip .

# Deploy binaries
az functionapp deployment source config-zip \
  --resource-group $resourceGroupName \
  --name $appServiceName \
  --src "publish.zip"

# Clean up
rm publish.zip
cd $currentDir

######
