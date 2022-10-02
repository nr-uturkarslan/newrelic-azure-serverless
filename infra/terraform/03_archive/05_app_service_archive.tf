### App Service - Archive ###

# Service Plan
resource "azurerm_service_plan" "archive" {
  name                = var.service_plan_name_archive
  location            = azurerm_resource_group.archive.location
  resource_group_name = azurerm_resource_group.archive.name

  os_type  = "Linux"
  sku_name = "S1"
}

# App Service
resource "azurerm_linux_web_app" "archive" {
  name                = var.app_service_name_archive
  resource_group_name = azurerm_resource_group.archive.name
  location            = azurerm_resource_group.archive.location

  service_plan_id = azurerm_service_plan.archive.id

  app_settings = {

    # Container
    # WEBSITES_CONTAINER_START_TIME_LIMIT = "1800"
    # WEBSITES_PORT = "80"

    # Container Registry
    DOCKER_REGISTRY_SERVER_URL = data.azurerm_container_registry.platform.login_server
    DOCKER_REGISTRY_SERVER_USERNAME = data.azurerm_container_registry.platform.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD = data.azurerm_container_registry.platform.admin_password

    # Blob Container
    BLOB_CONTAINER_URI = "test"

    # Service Bus
    SERVICE_BUS_FQDN       = "${data.azurerm_servicebus_namespace.platform.name}.servicebus.windows.net"
    SERVICE_BUS_QUEUE_NAME = data.azurerm_servicebus_queue.archive.name

    # # New Relic
    # NEW_RELIC_APP_NAME              = "ArchiveService"
    # NEW_RELIC_LICENSE_KEY           = var.new_relic_license_key
    # CORECLR_ENABLE_PROFILING        = "1"
    # CORECLR_PROFILER                = "{36032161-FFC0-4B61-B559-F6C5D41BAE5A}"
    # CORECLR_PROFILER_PATH           = "/home/site/wwwroot/newrelic/libNewRelicProfiler.so"
    # CORECLR_NEWRELIC_HOME           = "/home/site/wwwroot/newrelic"
    # NEWRELIC_PROFILER_LOG_DIRECTORY = "/home/LogFiles/NewRelic"
  }

  site_config {
    application_stack {
      # docker_image     = "${data.azurerm_container_registry.platform.name}.azurecr.io/archive"
      # docker_image_tag = "1663278324"
      
      # Random image for first deployment
      docker_image     = "mcr.microsoft.com/oss/nginx/nginx"
      docker_image_tag = "1.15.5-alpine"
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
  scope                            = data.azurerm_container_registry.platform.id
  skip_service_principal_aad_check = true
}

# Service Bus - Archive
resource "azurerm_role_assignment" "data_receiver_for_archive_as_on_service_queue_archive" {
  scope                = data.azurerm_servicebus_queue.archive.id
  role_definition_name = "Azure Service Bus Data Receiver"
  principal_id         = azurerm_linux_web_app.archive.identity[0].principal_id
}
