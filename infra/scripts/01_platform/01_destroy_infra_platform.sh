#!/bin/bash

###################
### Infra Setup ###
###################

### Set parameters
project="nr1"
locationLong="westeurope"
locationShort="euw"
stageLong="dev"
stageShort="d"
instance="001"

platform="platform"

### Set variables

# Platform
resourceGroupNamePlatform="rg${project}${locationShort}${platform}${stageShort}${instance}"
containerRegistryNamePlatform="acr${project}${locationShort}${platform}${stageShort}${instance}"
cosmosDbAccountNamePlatform="cdb${project}${locationShort}${platform}${stageShort}${instance}"
serviceBusNamespaceNamePlatform="sb${project}${locationShort}${platform}${stageShort}${instance}"
storageAccountNamePlatform="st${project}${locationShort}${platform}${stageShort}${instance}"
applicationInsightsNamePlatform="appins${project}${locationShort}${platform}${stageShort}${instance}"

### Terraform destroy
terraform -chdir=../terraform/01_platform destroy \
  -var project=$project \
  -var location_long=$locationLong \
  -var location_short=$locationShort \
  -var stage_short=$stageShort \
  -var stage_long=$stageLong \
  -var instance=$instance \
  -var resource_group_name_platform=$resourceGroupNamePlatform \
  -var container_registry_name_platform=$containerRegistryNamePlatform \
  -var cosmos_db_account_name_platform=$cosmosDbAccountNamePlatform \
  -var cosmos_db_name_device=$cosmosDbNameDevice \
  -var service_bus_namespace_name_platform=$serviceBusNamespaceNamePlatform \
  -var service_bus_queue_name_archive=$serviceBusQueueNameArchive \
  -var storage_account_name_platform=$storageAccountNamePlatform \
  -var blob_container_name_archive=$blobContainerNameArchive \
  -var application_insights_name_platform=$applicationInsightsNamePlatform
