### Resource Group ###

# Resource Group - Archive
resource "azurerm_resource_group" "archive" {
  name     = var.resource_group_name_archive
  location = data.azurerm_resource_group.platform.location
}
