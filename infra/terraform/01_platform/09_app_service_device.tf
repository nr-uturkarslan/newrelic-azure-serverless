### App Service - Device ###

# Service Plan
resource "azurerm_service_plan" "device" {
  name                = var.service_plan_name_device
  location            = azurerm_resource_group.device.location
  resource_group_name = azurerm_resource_group.device.name

  os_type  = "Linux"
  sku_name = "B1"
}

# App Service
resource "azurerm_linux_web_app" "device" {
  name                = var.app_service_name_device
  resource_group_name = azurerm_resource_group.device.name
  location            = azurerm_resource_group.device.location

  service_plan_id = azurerm_service_plan.device.id

  app_settings = {

    # Cosmos DB
    COSMOS_DB_URI            = azurerm_cosmosdb_account.platform.endpoint
    COSMOS_DB_NAME           = azurerm_cosmosdb_sql_database.device.name
    COSMOS_DB_CONTAINER_NAME = azurerm_cosmosdb_sql_container.device.name

    # New Relic
    NEW_RELIC_APP_NAME              = "DeviceService"
    NEW_RELIC_LICENSE_KEY           = var.new_relic_license_key
    CORECLR_ENABLE_PROFILING        = "1"
    CORECLR_PROFILER                = "{36032161-FFC0-4B61-B559-F6C5D41BAE5A}"
    CORECLR_PROFILER_PATH           = "/home/site/wwwroot/newrelic/libNewRelicProfiler.so"
    CORECLR_NEWRELIC_HOME           = "/home/site/wwwroot/newrelic"
    NEWRELIC_PROFILER_LOG_DIRECTORY = "/home/LogFiles/NewRelic"
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
  resource_group_name = azurerm_resource_group.platform.name
  account_name        = azurerm_cosmosdb_account.platform.name

  role_definition_id = azurerm_cosmosdb_sql_role_definition.contributor.id
  principal_id       = azurerm_linux_web_app.device.identity[0].principal_id
  scope              = "/subscriptions/${data.azurerm_client_config.current.subscription_id}/resourceGroups/${azurerm_resource_group.platform.name}/providers/Microsoft.DocumentDB/databaseAccounts/${azurerm_cosmosdb_account.platform.name}"
}
