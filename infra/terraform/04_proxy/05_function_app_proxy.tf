### Function App - Proxy ###

# Service Plan
resource "azurerm_service_plan" "proxy" {
  name                = var.service_plan_name_proxy
  resource_group_name = azurerm_resource_group.proxy.name
  location            = azurerm_resource_group.proxy.location

  os_type  = "Linux"
  sku_name = "Y1"
}

# Function App
resource "azurerm_linux_function_app" "proxy" {
  name                = var.function_app_name_proxy
  resource_group_name = azurerm_resource_group.proxy.name
  location            = azurerm_resource_group.proxy.location

  storage_account_name       = data.azurerm_storage_account.platform.name
  storage_account_access_key = data.azurerm_storage_account.platform.primary_access_key
  service_plan_id            = azurerm_service_plan.proxy.id

  app_settings = {

    # Application Insights
    APPINSIGHTS_INSTRUMENTATIONKEY = data.azurerm_application_insights.platform.instrumentation_key

    # Device Service
    DEVICE_SERVICE_URI = "https://${data.azurerm_linux_web_app.device.name}.azurewebsites.net"

    # Archive Service
    ARCHIVE_SERVICE_URI = "https://${data.azurerm_linux_web_app.archive.name}.azurewebsites.net"

    # Open Telemetry
    NEW_RELIC_APP_NAME                    = "ProxyService"
    NEW_RELIC_LICENSE_KEY                 = var.new_relic_license_key
    NEW_RELIC_OTEL_EXPORTER_OTLP_ENDPOINT = var.new_relic_otlp_export_endpoint
  }

  site_config {}

  identity {
    type = "SystemAssigned"
  }
}
