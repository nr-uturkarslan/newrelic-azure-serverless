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

  site_config {}

  identity {
    type = "SystemAssigned"
  }
}
