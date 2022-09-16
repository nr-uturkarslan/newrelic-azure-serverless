### Data ###

# Resource Group - Platform
data "azurerm_resource_group" "platform" {
  name = var.resource_group_name_platform
}

# Cosmos DB Account - Platform
data "azurerm_storage_account" "platform" {
  name                = var.storage_account_name_platform
  resource_group_name = var.resource_group_name_platform
}

# Application Insights - Platform
data "azurerm_application_insights" "platform" {
  name                = var.application_insights_name_platform
  resource_group_name = var.resource_group_name_platform
}

# App Service - Device
data "azurerm_linux_web_app" "device" {
  name                = var.app_service_name_device
  resource_group_name = var.resource_group_name_device
}

# App Service - Archive
data "azurerm_linux_web_app" "archive" {
  name                = var.app_service_name_archive
  resource_group_name = var.resource_group_name_archive
}
