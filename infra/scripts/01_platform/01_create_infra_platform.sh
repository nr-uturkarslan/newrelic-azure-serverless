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
containerRegistryNamePlatform="acr${project}${locationShort}${platform}${stageShort}${instance}"
cosmosDbAccountNamePlatform="cdb${project}${locationShort}${platform}${stageShort}${instance}"
serviceBusNamespaceNamePlatform="sb${project}${locationShort}${platform}${stageShort}${instance}"
storageAccountNamePlatform="st${project}${locationShort}${platform}${stageShort}${instance}"
applicationInsightsNamePlatform="appins${project}${locationShort}${platform}${stageShort}${instance}"

# Device
cosmosDbNameDevice="device"

# Archive
serviceBusQueueNameArchive="archive"
blobContainerNameArchive="archive"

### Shared Terraform storage account

# Resource group
echo "Checking shared resource group [${resourceGroupNameShared}]..."
sharedResourceGroup=$(az group show \
  --name $resourceGroupNameShared \
  2> /dev/null)

if [[ $sharedResourceGroup == "" ]]; then
  echo " -> Shared resource group does not exist. Creating..."

  sharedResourceGroup=$(az group create \
    --name $resourceGroupNameShared \
    --location $locationLong)

  echo -e " -> Shared resource group is created successfully.\n"
else
  echo -e " -> Shared resource group already exists.\n"
fi

# Storage account
echo "Checking shared storage account [${storageAccountNameShared}]..."
sharedStorageAccount=$(az storage account show \
    --resource-group $resourceGroupNameShared \
    --name $storageAccountNameShared \
  2> /dev/null)

if [[ $sharedStorageAccount == "" ]]; then
  echo " -> Shared storage account does not exist. Creating..."

  sharedStorageAccount=$(az storage account create \
    --resource-group $resourceGroupNameShared \
    --name $storageAccountNameShared \
    --sku "Standard_LRS" \
    --encryption-services "blob")

  echo -e " -> Shared storage account is created successfully.\n"
else
  echo -e " -> Shared storage account already exists.\n"
fi

# Terraform blob container
echo "Checking Terraform blob container [${project}]..."
terraformBlobContainer=$(az storage container show \
  --account-name $storageAccountNameShared \
  --name $project \
  2> /dev/null)

if [[ $terraformBlobContainer == "" ]]; then
  echo " -> Terraform blob container does not exist. Creating..."

  terraformBlobContainer=$(az storage container create \
    --account-name $storageAccountNameShared \
    --name $project \
    2> /dev/null)

  echo -e " -> Terraform blob container is created successfully.\n"
else
  echo -e " -> Terraform blob container already exists.\n"
fi

### Terraform deployment
azureAccount=$(az account show)
tenantId=$(echo $azureAccount | jq .tenantId)
subscriptionId=$(echo $azureAccount | jq .id)

echo -e 'tenant_id='"${tenantId}"'
subscription_id='"${subscriptionId}"'
resource_group_name=''"'${resourceGroupNameShared}'"''
storage_account_name=''"'${storageAccountNameShared}'"''
container_name=''"'${project}'"''
key=''"'${platform}${stageShort}${instance}.tfstate'"''' \
> ../../terraform/01_platform/backend.config

terraform -chdir=../../terraform/01_platform init --backend-config="./backend.config"

terraform -chdir=../../terraform/01_platform plan \
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
  -var application_insights_name_platform=$applicationInsightsNamePlatform \
  -out "./tfplan"

terraform -chdir=../../terraform/01_platform apply tfplan
