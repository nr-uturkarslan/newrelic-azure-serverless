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

    # New Relic
    NEW_RELIC_APP_NAME              = "ArchiveService"
    NEW_RELIC_LICENSE_KEY           = var.newRelicLicenseKey
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
