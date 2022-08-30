### Cosmos DB ###

# Cosmod DB Account
resource "azurerm_cosmosdb_account" "nr1" {
  name                = var.project_cosmos_db_account_name
  resource_group_name = azurerm_resource_group.nr1.name
  location            = azurerm_resource_group.nr1.location

  offer_type = "Standard"
  kind       = "GlobalDocumentDB"

  enable_free_tier = true

  consistency_policy {
    consistency_level = "Strong"
  }

  geo_location {
    location          = azurerm_resource_group.nr1.location
    failover_priority = 0
  }
}

# Cosmos DB - SQL DB
resource "azurerm_cosmosdb_sql_database" "device" {
  name                = var.project_cosmos_db_name_device
  resource_group_name = azurerm_resource_group.nr1.name

  account_name = azurerm_cosmosdb_account.nr1.name
  throughput   = 400
}

resource "azurerm_cosmosdb_sql_container" "device" {
  name                = "device"
  resource_group_name = azurerm_resource_group.nr1.name

  account_name  = azurerm_cosmosdb_account.nr1.name
  database_name = azurerm_cosmosdb_sql_database.device.name

  partition_key_path    = "/id"
  partition_key_version = 1
  throughput            = 400

  indexing_policy {
    indexing_mode = "consistent"
  }

  # unique_key {
  #   paths = ["/id"]
  # }
}

# SQL Role Definition
resource "azurerm_cosmosdb_sql_role_definition" "contributor" {
  name                = "contributor"
  resource_group_name = azurerm_resource_group.nr1.name
  account_name        = azurerm_cosmosdb_account.nr1.name

  type = "CustomRole"
  assignable_scopes = [
    "/subscriptions/${data.azurerm_client_config.current.subscription_id}/resourceGroups/${azurerm_resource_group.nr1.name}/providers/Microsoft.DocumentDB/databaseAccounts/${azurerm_cosmosdb_account.nr1.name}"
  ]

  permissions {
    data_actions = [
      "Microsoft.DocumentDB/databaseAccounts/readMetadata",
      "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/items/*"
    ]
  }
}
