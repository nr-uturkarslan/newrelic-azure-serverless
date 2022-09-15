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

# New Relic License Key
variable "new_relic_license_key" {
  type = string
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

# Service Bus
variable "service_bus_namespace_name_platform" {
  type = string
}

# Storage Account
variable "storage_account_name_platform" {
  type = string
}
#########

###############
### Archive ###
###############

# Resource Group
variable "resource_group_name_archive" {
  type = string
}

# Service Plan
variable "service_plan_name_archive" {
  type = string
}

# App Service
variable "app_service_name_archive" {
  type = string
}

# Blob Container
variable "blob_container_name_archive" {
  type = string
}

# Service Bus Queue
variable "service_bus_queue_name_archive" {
  type = string
}
#########
