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

### Set variables

# Shared
sharedResourceGroupName="rg${project}${locationShort}${shared}x000"
sharedStorageAccountName="st${project}${locationShort}${shared}x000"

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

# ### Shared Terraform storage account

# Resource group
echo "Checking shared resource group [${sharedResourceGroupName}]..."
sharedResourceGroup=$(az group show \
  --name $sharedResourceGroupName \
  2> /dev/null)

if [[ $sharedResourceGroup == "" ]]; then
  echo " -> Shared resource group does not exist. Creating..."

  sharedResourceGroup=$(az group create \
    --name $sharedResourceGroupName \
    --location $locationLong)

  echo -e " -> Shared resource group is created successfully.\n"
else
  echo -e " -> Shared resource group already exists.\n"
fi

# Storage account
echo "Checking shared storage account [${sharedStorageAccountName}]..."
sharedStorageAccount=$(az storage account show \
    --resource-group $sharedResourceGroupName \
    --name $sharedStorageAccountName \
  2> /dev/null)

if [[ $sharedStorageAccount == "" ]]; then
  echo " -> Shared storage account does not exist. Creating..."

  sharedStorageAccount=$(az storage account create \
    --resource-group $sharedResourceGroupName \
    --name $sharedStorageAccountName \
    --sku "Standard_LRS" \
    --encryption-services "blob")

  echo -e " -> Shared storage account is created successfully.\n"
else
  echo -e " -> Shared storage account already exists.\n"
fi

# Terraform blob container
echo "Checking Terraform blob container [${project}]..."
terraformBlobContainer=$(az storage container show \
  --account-name $sharedStorageAccountName \
  --name $project \
  2> /dev/null)

if [[ $terraformBlobContainer == "" ]]; then
  echo " -> Terraform blob container does not exist. Creating..."

  terraformBlobContainer=$(az storage container create \
    --account-name $sharedStorageAccountName \
    --name $project \
    2> /dev/null)

  echo -e " -> Terraform blob container is created successfully.\n"
else
  echo -e " -> Terraform blob container already exists.\n"
fi

### Project Terraform deployment
azureAccount=$(az account show)
tenantId=$(echo $azureAccount | jq .tenantId)
subscriptionId=$(echo $azureAccount | jq .id)

echo -e 'tenant_id='"${tenantId}"'
subscription_id='"${subscriptionId}"'
resource_group_name=''"'${sharedResourceGroupName}'"''
storage_account_name=''"'${sharedStorageAccountName}'"''
container_name=''"'${project}'"''
key=''"'${stageShort}${instance}.tfstate'"''' \
> ../terraform/01_platform/backend.config

terraform -chdir=../terraform/01_platform init --backend-config="./backend.config"

terraform -chdir=../terraform/01_platform plan \
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
  -var project_function_app_name_proxy=$projectFunctionAppNameProxy \
  -out "./tfplan"

terraform -chdir=../terraform/01_platform apply tfplan
