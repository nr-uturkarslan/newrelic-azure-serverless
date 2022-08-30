resource "azurerm_service_plan" "proxy" {
  name                = var.project_service_plan_name_proxy
  resource_group_name = azurerm_resource_group.nr1.name
  location            = azurerm_resource_group.nr1.location

  os_type  = "Linux"
  sku_name = "Y1"
}

resource "azurerm_linux_function_app" "proxy" {
  name                = var.project_function_app_name_proxy
  resource_group_name = azurerm_resource_group.nr1.name
  location            = azurerm_resource_group.nr1.location

  storage_account_name       = azurerm_storage_account.nr1.name
  storage_account_access_key = azurerm_storage_account.nr1.primary_access_key
  service_plan_id            = azurerm_service_plan.proxy.id

  app_settings = {

    # Device Service
    DEVICE_SERVICE_URI = azurerm_linux_web_app.device.outbound_ip_address_list[
      length(azurerm_linux_web_app.device.outbound_ip_address_list)-1
    ]

    # Archive Service
    ARCHIVE_SERVICE_URI = azurerm_linux_web_app.archive.outbound_ip_address_list[
      length(azurerm_linux_web_app.archive.outbound_ip_address_list)-1
    ]

    # New Relic
    NEW_RELIC_APP_NAME              = "ProxyService"
    NEW_RELIC_LICENSE_KEY           = var.newRelicLicenseKey
    CORECLR_ENABLE_PROFILING        = "1"
    CORECLR_PROFILER                = "{36032161-FFC0-4B61-B559-F6C5D41BAE5A}"
    CORECLR_PROFILER_PATH           = "/home/site/wwwroot/newrelic/libNewRelicProfiler.so"
    CORECLR_NEWRELIC_HOME           = "/home/site/wwwroot/newrelic"
    NEWRELIC_PROFILER_LOG_DIRECTORY = "/home/LogFiles/NewRelic"
  }
  site_config {}

  identity {
    type = "SystemAssigned"
  }
}
