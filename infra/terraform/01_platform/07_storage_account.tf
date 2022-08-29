### Storage Account ###

# Storage Account
resource "azurerm_storage_account" "nr1" {
  name                = var.project_storage_account_name
  resource_group_name = azurerm_resource_group.nr1.name
  location            = azurerm_resource_group.nr1.location

  account_tier             = "Standard"
  account_replication_type = "LRS"
}

# Blob Container - Archive
resource "azurerm_storage_container" "archive" {
  name                  = var.project_blob_container_name_archive
  storage_account_name  = azurerm_storage_account.nr1.name
  container_access_type = "private"
}
