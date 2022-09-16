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

shared="shared"
platform="platform"
archive="archive"

### Set variables

# Shared
resourceGroupNameShared="rg${project}${locationShort}${shared}x000"
storageAccountNameShared="st${project}${locationShort}${shared}x000"

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


### Terraform deployment
azureAccount=$(az account show)
tenantId=$(echo $azureAccount | jq .tenantId)
subscriptionId=$(echo $azureAccount | jq .id)

echo -e 'tenant_id='"${tenantId}"'
subscription_id='"${subscriptionId}"'
resource_group_name=''"'${resourceGroupNameShared}'"''
storage_account_name=''"'${storageAccountNameShared}'"''
container_name=''"'${project}'"''
key=''"'${archive}${stageShort}${instance}.tfstate'"''' \
> ../../terraform/03_archive/backend.config

terraform -chdir=../../terraform/03_archive init --backend-config="./backend.config"

terraform -chdir=../../terraform/03_archive plan \
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
  -var service_bus_queue_name_archive=$serviceBusQueueNameArchive \
  -out "./tfplan"

terraform -chdir=../../terraform/03_archive apply tfplan
