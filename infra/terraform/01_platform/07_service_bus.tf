### Service Bus ###

# Service Bus Namespace
resource "azurerm_servicebus_namespace" "platform" {
  name                = var.service_bus_namespace_name_platform
  resource_group_name = azurerm_resource_group.platform.name
  location            = azurerm_resource_group.platform.location
  sku                 = "Standard"
}

# Service Bus Queue - Archive
resource "azurerm_servicebus_queue" "archive" {
  name         = var.service_bus_queue_name_archive
  namespace_id = azurerm_servicebus_namespace.platform.id

  enable_partitioning = false
}
