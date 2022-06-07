param name string
param location string = resourceGroup().location
param queues string

var queueArray = empty(queues)? [] : split(queues, ',')

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
