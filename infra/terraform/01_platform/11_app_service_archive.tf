### App Service - Archive ###

# Service Plan
resource "azurerm_service_plan" "archive" {
  name                = var.service_plan_name_archive
  location            = azurerm_resource_group.archive.location
  resource_group_name = azurerm_resource_group.archive.name

  os_type  = "Linux"
  sku_name = "B1"
}

# App Service
resource "azurerm_linux_web_app" "archive" {
  name                = var.app_service_name_archive
  resource_group_name = azurerm_resource_group.archive.name
  location            = azurerm_resource_group.archive.location

  service_plan_id = azurerm_service_plan.archive.id

  app_settings = {

    # Blob Container
    BLOB_CONTAINER_URI = "test"

    # Service Bus
    SERVICE_BUS_FQDN       = "${azurerm_servicebus_namespace.platform.name}.servicebus.windows.net"
    SERVICE_BUS_QUEUE_NAME = azurerm_servicebus_queue.archive.name

    # New Relic
    NEW_RELIC_APP_NAME              = "ArchiveService"
    NEW_RELIC_LICENSE_KEY           = var.new_relic_license_key
    CORECLR_ENABLE_PROFILING        = "1"
    CORECLR_PROFILER                = "{36032161-FFC0-4B61-B559-F6C5D41BAE5A}"
    CORECLR_PROFILER_PATH           = "/home/site/wwwroot/newrelic/libNewRelicProfiler.so"
    CORECLR_NEWRELIC_HOME           = "/home/site/wwwroot/newrelic"
    NEWRELIC_PROFILER_LOG_DIRECTORY = "/home/LogFiles/NewRelic"

  }

  site_config {
    application_stack {
      docker_image     = "${var.container_registry_name_platform}.azurecr.io/archive"
      docker_image_tag = "latest"
    }
  }

  identity {
    type = "SystemAssigned"
  }
}

# Role Assignment AcrPull
resource "azurerm_role_assignment" "acr_pull_for_archive_as" {
  principal_id                     = azurerm_linux_web_app.archive.identity[0].principal_id
  role_definition_name             = "AcrPull"
  scope                            = azurerm_container_registry.platform.id
  skip_service_principal_aad_check = true
}
