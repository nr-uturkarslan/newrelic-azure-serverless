### App Service - Device ###

# Service Plan 
resource "azurerm_service_plan" "device" {
  name                = var.project_service_plan_name_device
  location            = azurerm_resource_group.nr1.location
  resource_group_name = azurerm_resource_group.nr1.name

  os_type  = "Linux"
  sku_name = "S1"
}

# App Service
resource "azurerm_linux_web_app" "device" {
  name                = var.project_app_service_name_device
  resource_group_name = azurerm_resource_group.nr1.name
  location            = azurerm_resource_group.nr1.location

  service_plan_id = azurerm_service_plan.device.id

  app_settings = {
    COSMOS_DB_URI            = azurerm_cosmosdb_account.nr1.endpoint
    COSMOS_DB_NAME           = azurerm_cosmosdb_sql_database.device.name
    COSMOS_DB_CONTAINER_NAME = azurerm_cosmosdb_sql_container.device.name
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

# SQL Role Assignment - Device
resource "azurerm_cosmosdb_sql_role_assignment" "contributor_for_device_service" {
  resource_group_name = azurerm_resource_group.nr1.name
  account_name        = azurerm_cosmosdb_account.nr1.name

  role_definition_id = azurerm_cosmosdb_sql_role_definition.contributor.id
  principal_id       = azurerm_linux_web_app.device.identity[0].principal_id
  scope              = "/subscriptions/${data.azurerm_client_config.current.subscription_id}/resourceGroups/${azurerm_resource_group.nr1.name}/providers/Microsoft.DocumentDB/databaseAccounts/${azurerm_cosmosdb_account.nr1.name}"
}
