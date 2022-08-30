### Resource Group ###

# Resource Group - Platform
resource "azurerm_resource_group" "platform" {
  name     = var.resource_group_name_platform
  location = var.location_long
}

# Resource Group - Device
resource "azurerm_resource_group" "device" {
  name     = var.resource_group_name_device
  location = azurerm_resource_group.platform.location
}

# Resource Group - Archive
resource "azurerm_resource_group" "archive" {
  name     = var.resource_group_name_archive
  location = azurerm_resource_group.platform.location
}

# Resource Group - Proxy
resource "azurerm_resource_group" "proxy" {
  name     = var.resource_group_name_proxy
  location = azurerm_resource_group.platform.location
}
