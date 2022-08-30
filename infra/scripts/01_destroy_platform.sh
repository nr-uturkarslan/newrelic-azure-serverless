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

proxy="proxy"
device="device"
archive="archive"

newRelicOtlpExportEndpoint="https://otlp.eu01.nr-data.net:4317"

### Set variables

# Shared
resourceGroupNameShared="rg${project}${locationShort}${shared}x000"
storageAccountNameShared="st${project}${locationShort}${shared}x000"

# Platform
resourceGroupNamePlatform="rg${project}${locationShort}${platform}${stageShort}${instance}"
cosmosDbAccountNamePlatform="cdb${project}${locationShort}${platform}${stageShort}${instance}"
serviceBusNamespaceNamePlatform="sb${project}${locationShort}${platform}${stageShort}${instance}"
storageAccountNamePlatform="st${project}${locationShort}${platform}${stageShort}${instance}"
applicationInsightsNamePlatform="appins${project}${locationShort}${platform}${stageShort}${instance}"

# Device
resourceGroupNameDevice="rg${project}${locationShort}${device}${stageShort}${instance}"
servicePlanNameDevice="plan${project}${locationShort}${device}${stageShort}${instance}"
appServiceNameDevice="as${project}${locationShort}${device}${stageShort}${instance}"
cosmosDbNameDevice="device"

# Archive
resourceGroupNameArchive="rg${project}${locationShort}${archive}${stageShort}${instance}"
projectServicePlanNameArchive="plan${project}${locationShort}${archive}${stageShort}${instance}"
projectAppServiceNameArchive="as${project}${locationShort}${archive}${stageShort}${instance}"
serviceBusQueueNameArchive="archive"
blobContainerNameArchive="archive"

# Proxy
resourceGroupNameProxy="rg${project}${locationShort}${proxy}${stageShort}${instance}"
servicePlanNameProxy="plan${project}${locationShort}${proxy}${stageShort}${instance}"
functionAppNameProxy="func${project}${locationShort}${proxy}${stageShort}${instance}"

### Terraform destroy

terraform -chdir=../terraform/01_platform destroy \
  -var project=$project \
  -var location_long=$locationLong \
  -var location_short=$locationShort \
  -var stage_short=$stageShort \
  -var stage_long=$stageLong \
  -var instance=$instance \
  -var new_relic_license_key=$NEWRELIC_LICENSE_KEY \
  -var new_relic_otlp_export_endpoint=$newRelicOtlpExportEndpoint \
  -var resource_group_name_platform=$resourceGroupNamePlatform \
  -var cosmos_db_account_name_platform=$cosmosDbAccountNamePlatform \
  -var service_bus_namespace_name_platform=$serviceBusNamespaceNamePlatform \
  -var storage_account_name_platform=$storageAccountNamePlatform \
  -var application_insights_name_platform=$applicationInsightsNamePlatform \
  -var resource_group_name_device=$resourceGroupNameDevice \
  -var service_plan_name_device=$servicePlanNameDevice \
  -var app_service_name_device=$appServiceNameDevice \
  -var cosmos_db_name_device=$cosmosDbNameDevice \
  -var resource_group_name_archive=$resourceGroupNameArchive \
  -var service_plan_name_archive=$projectServicePlanNameArchive \
  -var app_service_name_archive=$projectAppServiceNameArchive \
  -var blob_container_name_archive=$blobContainerNameArchive \
  -var service_bus_queue_name_archive=$serviceBusQueueNameArchive \
  -var resource_group_name_proxy=$resourceGroupNameProxy \
  -var service_plan_name_proxy=$servicePlanNameProxy \
  -var function_app_name_proxy=$functionAppNameProxy
