### Variables ###

## General

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

## Specific

# platform
variable "platform" {
  type    = string
  default = "platform"
}

# New Relic License Key
variable "newRelicLicenseKey" {
  type    = string
}

## Resource Names

# Resource Group
variable "project_resource_group_name" {
  type    = string
}

# Cosmos DB
variable "project_cosmos_db_account_name" {
  type    = string
}

variable "project_cosmos_db_name_device" {
  type    = string
}

# Service Bus
variable "project_service_bus_namespace_name" {
  type    = string
}

variable "project_service_bus_queue_name_archive" {
  type    = string
}

# Storage Account
variable "project_storage_account_name" {
  type    = string
}

variable "project_blob_container_name_archive" {
  type    = string
}

# App Service - Device Service
variable "project_service_plan_name_device" {
  type    = string
}

variable "project_app_service_name_device" {
  type    = string
}

# App Service - Archive Service
variable "project_service_plan_name_archive" {
  type    = string
}

variable "project_app_service_name_archive" {
  type    = string
}

# Function App - Proxy Service
variable "project_service_plan_name_proxy" {
  type    = string
}

variable "project_function_app_name_proxy" {
  type    = string
}