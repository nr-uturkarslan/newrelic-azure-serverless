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
device="device"

### Set variables

# Shared
resourceGroupNameShared="rg${project}${locationShort}${shared}x000"
storageAccountNameShared="st${project}${locationShort}${shared}x000"

# Platform
resourceGroupNamePlatform="rg${project}${locationShort}${platform}${stageShort}${instance}"
cosmosDbAccountNamePlatform="cdb${project}${locationShort}${platform}${stageShort}${instance}"
serviceBusNamespaceNamePlatform="sb${project}${locationShort}${platform}${stageShort}${instance}"

# Device
resourceGroupNameDevice="rg${project}${locationShort}${device}${stageShort}${instance}"
servicePlanNameDevice="plan${project}${locationShort}${device}${stageShort}${instance}"
appServiceNameDevice="as${project}${locationShort}${device}${stageShort}${instance}"
cosmosDbNameDevice="device"

# Archive
serviceBusQueueNameArchive="archive"

### Terraform deployment
azureAccount=$(az account show)
tenantId=$(echo $azureAccount | jq .tenantId)
subscriptionId=$(echo $azureAccount | jq .id)

echo -e 'tenant_id='"${tenantId}"'
subscription_id='"${subscriptionId}"'
resource_group_name=''"'${resourceGroupNameShared}'"''
storage_account_name=''"'${storageAccountNameShared}'"''
container_name=''"'${project}'"''
key=''"'${device}${stageShort}${instance}.tfstate'"''' \
> ../../terraform/02_device/backend.config

terraform -chdir=../../terraform/02_device init --backend-config="./backend.config"

terraform -chdir=../../terraform/02_device plan \
  -var project=$project \
  -var location_long=$locationLong \
  -var location_short=$locationShort \
  -var stage_short=$stageShort \
  -var stage_long=$stageLong \
  -var instance=$instance \
  -var new_relic_license_key=$NEWRELIC_LICENSE_KEY \
  -var resource_group_name_platform=$resourceGroupNamePlatform \
  -var cosmos_db_account_name_platform=$cosmosDbAccountNamePlatform \
  -var service_bus_namespace_name_platform=$serviceBusNamespaceNamePlatform \
  -var resource_group_name_device=$resourceGroupNameDevice \
  -var service_plan_name_device=$servicePlanNameDevice \
  -var app_service_name_device=$appServiceNameDevice \
  -var cosmos_db_name_device=$cosmosDbNameDevice \
  -var service_bus_queue_name_archive=$serviceBusQueueNameArchive \
  -out "./tfplan"

terraform -chdir=../../terraform/02_device apply tfplan
