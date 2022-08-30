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

  storage_account_name       = azurerm_storage_account.platform.name
  storage_account_access_key = azurerm_storage_account.platform.primary_access_key
  service_plan_id            = azurerm_service_plan.proxy.id

  app_settings = {

    # Device Service
    DEVICE_SERVICE_URI = azurerm_linux_web_app.device.outbound_ip_address_list[
      length(azurerm_linux_web_app.device.outbound_ip_address_list) - 1
    ]

    # Archive Service
    ARCHIVE_SERVICE_URI = azurerm_linux_web_app.archive.outbound_ip_address_list[
      length(azurerm_linux_web_app.archive.outbound_ip_address_list) - 1
    ]

    # Open Telemetry
    NEW_RELIC_APP_NAME          = "ProxyService"
    NR_LICENSE_KEY              = var.new_relic_license_key
    OTEL_EXPORTER_OTLP_ENDPOINT = "https://otlp.eu01.nr-data.net:4317"
  }
  site_config {}

  identity {
    type = "SystemAssigned"
  }
}
