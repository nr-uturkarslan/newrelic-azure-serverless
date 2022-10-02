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
app="archive"

### Set variables
resourceGroupName="rg${project}${locationShort}${app}${stageShort}${instance}"
containerRegistryNamePlatform="acr${project}${locationShort}${platform}${stageShort}${instance}"
appServiceName="as${project}${locationShort}${app}${stageShort}${instance}"

###########
### App ###
###########

# Docker tag
dockerTag="$(date +%s)"
echo $dockerTag

# ACR build
az acr build \
  --platform linux/amd64 \
  --build-arg newRelicAppName="ArchiveService" \
  --build-arg newRelicLicenseKey=$NEWRELIC_LICENSE_KEY \
  --registry $containerRegistryNamePlatform \
  --image "${app}:${dockerTag}" \
  "../../../apps/ArchiveService/ArchiveService/."

# Deploy
az webapp config container set \
  --resource-group $resourceGroupName \
  --name $appServiceName \
  --docker-custom-image-name "${containerRegistryNamePlatform}.azurecr.io/${app}:${dockerTag}"
######
