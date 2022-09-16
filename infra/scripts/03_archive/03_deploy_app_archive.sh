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

# Deploy
az webapp config container set \
  --resource-group $resourceGroupName \
  --name $appServiceName \
  --docker-custom-image-name "${containerRegistryNamePlatform}.azurecr.io/archive:1663280000"
######
