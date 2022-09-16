### Data ###

# Resource Group - Platform
data "azurerm_resource_group" "platform" {
  name = var.resource_group_name_platform
}

# Container Registry - Platform
data "azurerm_container_registry" "platform" {
  name                = var.container_registry_name_platform
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
