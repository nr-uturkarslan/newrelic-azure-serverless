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

platform="platform"
app="proxy"

### Set variables
resourceGroupName="rg${project}${locationShort}${app}${stageShort}${instance}"
functionAppName="func${project}${locationShort}${app}${stageShort}${instance}"

###########
### App ###
###########

# Publish code
dotnet publish \
  ../../apps/ProxyService/ProxyService/ProxyService.csproj \
  -c Release

# Zip binaries
currentDir=$(pwd)
cd ../../apps/ProxyService/ProxyService/bin/Release/net6.0/publish
zip -r publish.zip .

# Deploy binaries
az functionapp deployment source config-zip \
  --resource-group $resourceGroupName \
  --name $functionAppName \
  --src "publish.zip"

# Clean up
rm publish.zip
cd $currentDir

######
