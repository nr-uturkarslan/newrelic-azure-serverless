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
archive="archive"

### Set variables

# Platform
resourceGroupNamePlatform="rg${project}${locationShort}${platform}${stageShort}${instance}"
containerRegistryNamePlatform="acr${project}${locationShort}${platform}${stageShort}${instance}"
serviceBusNamespaceNamePlatform="sb${project}${locationShort}${platform}${stageShort}${instance}"

# Archive
resourceGroupNameArchive="rg${project}${locationShort}${archive}${stageShort}${instance}"
projectServicePlanNameArchive="plan${project}${locationShort}${archive}${stageShort}${instance}"
projectAppServiceNameArchive="as${project}${locationShort}${archive}${stageShort}${instance}"
serviceBusQueueNameArchive="archive"
blobContainerNameArchive="archive"

### Terraform destroy

terraform -chdir=../terraform/01_platform destroy \
  -var project=$project \
  -var location_long=$locationLong \
  -var location_short=$locationShort \
  -var stage_short=$stageShort \
  -var stage_long=$stageLong \
  -var instance=$instance \
  -var new_relic_license_key=$NEWRELIC_LICENSE_KEY \
  -var resource_group_name_platform=$resourceGroupNamePlatform \
  -var container_registry_name_platform=$containerRegistryNamePlatform \
  -var service_bus_namespace_name_platform=$serviceBusNamespaceNamePlatform \
  -var storage_account_name_platform=$storageAccountNamePlatform \
  -var resource_group_name_archive=$resourceGroupNameArchive \
  -var service_plan_name_archive=$projectServicePlanNameArchive \
  -var app_service_name_archive=$projectAppServiceNameArchive \
  -var blob_container_name_archive=$blobContainerNameArchive \
  -var service_bus_queue_name_archive=$serviceBusQueueNameArchive
