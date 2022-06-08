param name string
param location string = resourceGroup().location
param queues string
param tables string

var queueArray = empty(queues)? [] : split(queues, ',')
var tableArray = empty(tables)? [] : split(tables, ',')

@allowed([
  'nonprod'
  'prod'
])
param storageType string = 'nonprod'

var storageSku = {
  nonprod: 'Standard_LRS'
  prod: 'Standard_GRS'
}

resource stg 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: name
  location: location
  kind: 'StorageV2'
  sku: {
    name: storageSku[storageType]
  }
}

resource queueService 'Microsoft.Storage/storageAccounts/queueServices@2021-08-01' = if (!empty(queueArray)) {
  name: '${name}/default'
  dependsOn: [
    stg
  ]
  resource aaa 'queues' = [for q in queueArray: {
    name: q
  }]
}

resource tableService 'Microsoft.Storage/storageAccounts/tableServices@2021-08-01' = if (!empty(tableArray)) {
  name: '${name}/default'
  dependsOn: [
    stg
  ]
  resource storageTable 'tables' = [for t in tableArray: {
    name: t
  }]
}
