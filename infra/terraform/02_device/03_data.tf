### Data ###

# Client
data "azurerm_client_config" "current" {

}

# Resource Group - Platform
data "azurerm_resource_group" "platform" {
  name = var.resource_group_name_platform
}

# Cosmos DB Account - Platform
data "azurerm_cosmosdb_account" "platform" {
  name                = var.cosmos_db_account_name_platform
  resource_group_name = var.resource_group_name_platform
}

# Service Bus - Platform
data "azurerm_servicebus_namespace" "platform" {
  name                = var.service_bus_namespace_name_platform
  resource_group_name = var.resource_group_name_platform
}

# Service Bus Queue - Archive
data "azurerm_servicebus_queue" "archive" {
  name                = var.service_bus_queue_name_archive
  resource_group_name = var.resource_group_name_platform
  namespace_name      = var.service_bus_namespace_name_platform
}
