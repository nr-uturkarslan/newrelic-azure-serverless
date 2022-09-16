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

# Cosmos DB Account
variable "cosmos_db_account_name_platform" {
  type = string
}

# Service Bus
variable "service_bus_namespace_name_platform" {
  type = string
}
#########

##############
### Device ###
##############

# Resource Group
variable "resource_group_name_device" {
  type = string
}

# Service Plan
variable "service_plan_name_device" {
  type = string
}

# App Service
variable "app_service_name_device" {
  type = string
}

# Cosmos DB
variable "cosmos_db_name_device" {
  type = string
}
#########

##############
### Device ###
##############

# Service Bus Queue
variable "service_bus_queue_name_archive" {
  type = string
}
#########
