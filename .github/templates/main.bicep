param buildNumber string
param appName string
param environmentName string
param location string = resourceGroup().location
param containerImage string
param hotelCancellationQueue string
param pollingInSeconds int
param visibilityInSeconds int
param cancellationsTable string

var storageName = 'sg${appName}${environmentName}'
var aciName = 'aci-${appName}-${environmentName}'

module storageAccount 'storageaccount/template.bicep' = {
  name: '${buildNumber}-storage-account'
  params: {
    location: location
    name: storageName
    queues: hotelCancellationQueue
    tables: cancellationsTable
  }
}

module containerInstance 'aci/template.bicep' = {
  name: '${buildNumber}-container-instance'
  params: {
    location: location
    name: aciName
    dnsName: '${appName}-${environmentName}'
    image: containerImage
    hotelCancellationQueue: hotelCancellationQueue
    pollingSeconds: pollingInSeconds
    visibilityInSeconds: visibilityInSeconds
    storageAccount: storageName 
    cancellationsTable: cancellationsTable 
  }
  dependsOn: [
    storageAccount
  ]
}

module rbacqueue 'rbac/template.bicep'= {
  name: '${appName}-rbacqueues'
  params: {    
    accessibility: 'queue_read_write'
    friendlyName: '${appName}queueaccess'
    principalId: containerInstance.outputs.managedId
    storageAccountName: storageName
  }
  dependsOn:[
    containerInstance
    storageAccount
  ]
}

module rbactables 'rbac/template.bicep'= {
  name: '${appName}-rbactables'
  params: {    
    accessibility: 'table_write'
    friendlyName: '${appName}tableaccess'
    principalId: containerInstance.outputs.managedId
    storageAccountName: storageName
  }
  dependsOn:[
    containerInstance
    storageAccount
  ]
}
