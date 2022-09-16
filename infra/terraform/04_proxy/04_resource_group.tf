### Resource Group ###

# Resource Group - Proxy
resource "azurerm_resource_group" "proxy" {
  name     = var.resource_group_name_proxy
  location = data.azurerm_resource_group.platform.location
}
