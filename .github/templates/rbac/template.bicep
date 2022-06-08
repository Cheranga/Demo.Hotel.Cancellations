param storageAccountName string
param friendlyName string

@allowed([
  'queue_read_write'
  'table_write'
])
param accessibility string

@secure()
param principalId string

var roleDefinitions = {
  queue_read_write: '974c5e8b-45b9-4653-ba55-5f855dd0fb88' //  storage queue data contributor
  table_write: '0a9a7e1f-b9d0-4cc4-a60d-0319b160aaa3' //  Storage Table Data Contributor
}

resource role 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: roleDefinitions[accessibility]
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-02-01' existing = {
  scope: resourceGroup()  
  name: storageAccountName
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(resourceGroup().id, friendlyName, role.id)  
  scope:storageAccount
  properties: {
    roleDefinitionId: role.id
    principalId: principalId
    principalType: 'ServicePrincipal'
  }  
}

