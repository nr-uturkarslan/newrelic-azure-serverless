### Container Registry ###

# Container Registry
resource "azurerm_container_registry" "platform" {
  name                = var.container_registry_name_platform
  resource_group_name = azurerm_resource_group.platform.name
  location            = azurerm_resource_group.platform.location
  sku                 = "Standard"
  admin_enabled       = true
}
