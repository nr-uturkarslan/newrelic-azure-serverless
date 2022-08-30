### App Service - Archive ###

# Service Plan 
resource "azurerm_service_plan" "archive" {
  name                = var.project_service_plan_name_archive
  location            = azurerm_resource_group.nr1.location
  resource_group_name = azurerm_resource_group.nr1.name

  os_type  = "Linux"
  sku_name = "S1"
}

# App Service
resource "azurerm_linux_web_app" "archive" {
  name                = var.project_app_service_name_archive
  resource_group_name = azurerm_resource_group.nr1.name
  location            = azurerm_resource_group.nr1.location

  service_plan_id = azurerm_service_plan.archive.id

  app_settings = {
    # COSMOS_DB_URI            = azurerm_cosmosdb_account.nr1.endpoint
    # COSMOS_DB_NAME           = azurerm_cosmosdb_sql_database.archive.name
    # COSMOS_DB_CONTAINER_NAME = azurerm_cosmosdb_sql_container.archive.name
  }

  site_config {
    application_stack {
      dotnet_version = "6.0"
    }
  }

  identity {
    type = "SystemAssigned"
  }
}
