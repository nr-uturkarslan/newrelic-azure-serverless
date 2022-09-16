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

# New Relic OTLP Export Endpoint
variable "new_relic_otlp_export_endpoint" {
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

# Storage Account
variable "storage_account_name_platform" {
  type = string
}

# Application Insights
variable "application_insights_name_platform" {
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

# App Service
variable "app_service_name_device" {
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

# App Service
variable "app_service_name_archive" {
  type = string
}
#########

#############
### Proxy ###
#############

# Resource Group
variable "resource_group_name_proxy" {
  type = string
}

# Service Plan
variable "service_plan_name_proxy" {
  type = string
}

# Function App
variable "function_app_name_proxy" {
  type = string
}
#########
