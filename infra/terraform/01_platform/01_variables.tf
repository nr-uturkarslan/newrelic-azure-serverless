### Variables ###

###############
### General ###
###############

# project
variable "project" {
  type    = string
  default = "nr1"
}

# location_long
variable "location_long" {
  type    = string
  default = "westeurope"
}

# location_short
variable "location_short" {
  type    = string
  default = "euw"
}

# stage_long
variable "stage_long" {
  type    = string
  default = "dev"
}

# stage_short
variable "stage_short" {
  type    = string
  default = "d"
}

# instance
variable "instance" {
  type    = string
  default = "001"
}
#########

################
### Specific ###
################

# platform
variable "platform" {
  type    = string
  default = "platform"
}
#########

################
### Platform ###
################

# Resource Group
variable "resource_group_name_platform" {
  type = string
}

# Container Registry
variable "container_registry_name_platform" {
  type = string
}

# Cosmos DB Account
variable "cosmos_db_account_name_platform" {
  type = string
}

# Cosmos DB
variable "cosmos_db_name_device" {
  type = string
}

# Service Bus Namespace
variable "service_bus_namespace_name_platform" {
  type = string
}

# Service Bus Queue - Archive
variable "service_bus_queue_name_archive" {
  type = string
}

# Storage Account
variable "storage_account_name_platform" {
  type = string
}

# Blob Container - Archive
variable "blob_container_name_archive" {
  type = string
}

# Application Insights
variable "application_insights_name_platform" {
  type = string
}
#########
