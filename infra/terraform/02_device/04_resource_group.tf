### Resource Group ###

# Resource Group - Device
resource "azurerm_resource_group" "device" {
  name     = var.resource_group_name_device
  location = data.azurerm_resource_group.platform.location
}
