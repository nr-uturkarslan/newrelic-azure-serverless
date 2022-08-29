### Service Bus ###

# Service Bus Namespace
resource "azurerm_servicebus_namespace" "nr1" {
  name                = var.project_service_bus_namespace_name
  resource_group_name = azurerm_resource_group.nr1.name
  location            = azurerm_resource_group.nr1.location
  sku                 = "Standard"
}

# Service Bus Queue - Archive
resource "azurerm_servicebus_queue" "archive" {
  name         = var.project_service_bus_queue_name_archive
  namespace_id = azurerm_servicebus_namespace.nr1.id

  enable_partitioning = false
}
