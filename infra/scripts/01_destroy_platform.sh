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

### Set variables

# Platform
projectResourceGroupName="rg${project}${locationShort}${platform}${stageShort}${instance}"

projectCosmosDbAccountName="cdb${project}${locationShort}${platform}${stageShort}${instance}"
projectCosmosDbNameDevice="device"

projectServiceBusNamespaceName="sb${project}${locationShort}${platform}${stageShort}${instance}"
projectServiceBusQueueNameArchive="archive"

projectStorageAccountName="st${project}${locationShort}${platform}${stageShort}${instance}"
projectBlobContainerNameArchive="archive"

# Proxy
projectServicePlanNameProxy="plan${project}${locationShort}${proxy}${stageShort}${instance}"
projectFunctionAppNameProxy="func${project}${locationShort}${proxy}${stageShort}${instance}"

# Device
projectServicePlanNameDevice="plan${project}${locationShort}${device}${stageShort}${instance}"
projectAppServiceNameDevice="as${project}${locationShort}${device}${stageShort}${instance}"

# Archive
projectServicePlanNameArchive="plan${project}${locationShort}${archive}${stageShort}${instance}"
projectAppServiceNameArchive="as${project}${locationShort}${archive}${stageShort}${instance}"

### Terraform destroy

terraform -chdir=../terraform/01_platform destroy \
  -var project=$project \
  -var location_long=$locationLong \
  -var location_short=$locationShort \
  -var stage_short=$stageShort \
  -var stage_long=$stageLong \
  -var instance=$instance \
  -var newRelicLicenseKey=$NEWRELIC_LICENSE_KEY \
  -var project_resource_group_name=$projectResourceGroupName \
  -var project_cosmos_db_account_name=$projectCosmosDbAccountName \
  -var project_cosmos_db_name_device=$projectCosmosDbNameDevice \
  -var project_service_bus_namespace_name=$projectServiceBusNamespaceName \
  -var project_service_bus_queue_name_archive=$projectServiceBusQueueNameArchive \
  -var project_storage_account_name=$projectStorageAccountName \
  -var project_blob_container_name_archive=$projectBlobContainerNameArchive \
  -var project_service_plan_name_device=$projectServicePlanNameDevice \
  -var project_app_service_name_device=$projectAppServiceNameDevice \
  -var project_service_plan_name_archive=$projectServicePlanNameArchive \
  -var project_app_service_name_archive=$projectAppServiceNameArchive \
  -var project_service_plan_name_proxy=$projectServicePlanNameProxy \
  -var project_function_app_name_proxy=$projectFunctionAppNameProxy
