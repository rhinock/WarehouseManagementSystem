# Warehouse Management System

The system provides next functionality:
- view list of warehouses, items and warehouse items
- add or edit an item with particular price
- add, edit or delete an item to/from warehouse with particular amount

**Note**. Warehouses, Items and WarehouseItems -- are different tables.

Languages, technologies, instruments, etc.:
- OOP and SOLID principles
- only server side (WebAPI controllers)
- .NET Core
- EF Core
- PostgreSQL
- integration tests (webhost, xUnit)
- docker
- docker-compose
- kubernetes (minikube)
- helm
- curl

# PostgreSQL
## Diagram

![](Diagram.png)

## Initial Data
### Items

| Id | Name          | Price |
|:--:|:-------------:|:-----:|
| 1  | pencil        | 10.00 |
| 2  | pen           | 20.00 |
| 3  | felt-tip pen  | 30.00 |

### Warehouses

| Id | Name      | MaximumItems  |
|:--:|:---------:|:-------------:|
| 1  | miniature | 100           |
| 2  | decent    | 10000         |
| 3  | hefty     | 1000000       |

### WarehouseItems

| Id | WarehouseId   | ItemId    | Count     |
|:--:|:-------------:|:---------:|:---------:|
| 1  | 1             | 1         | 50        |
| 2  | 2             | 2         | 5000      |
| 3  | 3             | 3         | 500000    |

# Docker

- [Get Docker](https://docs.docker.com/get-docker/)
- [Dockerfile reference](https://docs.docker.com/engine/reference/builder/)
- [Dockerize an ASP.NET Core application](https://docs.docker.com/samples/dotnetcore/)
  - Dockerfile

# Docker Compose  

- [Install Docker Compose](https://docs.docker.com/compose/install/)
- [Get started with Docker Compose](https://docs.docker.com/compose/gettingstarted)
- [How to dockerize my dotnet core + postgresql app?](https://stackoverflow.com/a/57031267/16420746)
- [Docker-compose override not overriding connection string](https://stackoverflow.com/a/57336879/16420746)
  - [docker-compose.yml](docker-compose.yml)
    - images:
      - [postgres:12-alpine](https://hub.docker.com/layers/postgres/library/postgres/12-alpine/images/sha256-0de076b9b81cdb85391b222ce035c8b829544250c3fdbf6b527cb43f527dcf47?context=explore)
      - [rhinock/wms:latest](https://hub.docker.com/repository/docker/rhinock/wms)
    - db-data (data for postgres)
      - volume for docker-compose
      - for database recreating should be removed manually

# Kubernetes (minikube)

- [minikube start](https://minikube.sigs.k8s.io/docs/start/)
- [Hack the StorageClass](https://platform9.com/blog/tutorial-dynamic-provisioning-of-persistent-storage-in-kubernetes-with-minikube/)
  - [storageClass.yaml](k8s/storageClass.yaml)
- [StatefulSets Components](https://kubernetes.io/docs/concepts/workloads/controllers/statefulset/#components)
  - [postgres-statefulset.yaml](k8s/postgres-statefulset.yaml)
  - [postgres-service.yaml](k8s/postgres-service.yaml)
- [Connectionstring that an pod should use to connect to an postgresql pod in same cluster?](https://stackoverflow.com/a/60156175/16420746)
  - [wms-deployment.yml](k8s/wms-deployment.yml)
  - [wms-service.yml](k8s/wms-service.yml)
- [The Ingress resource](https://kubernetes.io/docs/concepts/services-networking/ingress/#the-ingress-resource)
- [Ingress Path Matching](https://kubernetes.github.io/ingress-nginx/user-guide/ingress-path-matching/)
  - [ingress.yaml](k8s/ingress.yaml)

## hosts file
```sh
# <minikube ip> wms.com
192.168.49.2 wms.com
```

# Helm

- [Installing Helm](https://helm.sh/docs/intro/install/)
- [Getting Started](https://helm.sh/docs/chart_template_guide/getting_started/)
- [Helm Create](https://helm.sh/docs/helm/helm_create/)
- [Built-in Objects](https://helm.sh/docs/chart_template_guide/builtin_objects/)
  - `{{ .Chart.Name }}`
- [Values not exist with loop over a range](https://github.com/helm/helm/issues/1311#issuecomment-252536380)
  - `{{- $root := . -}}`
  - `{{ $root.Values.service.port }}`
- [Kubernetes Helm, combine two variables with a string in the middle](https://stackoverflow.com/a/62874276/16420746)
  - `"{{ $root.Chart.Name }}-service"`    
- [Helm Install](https://helm.sh/docs/helm/helm_install/)
- [Helm Uninstall](https://helm.sh/docs/helm/helm_uninstall/)

# RESTful APIs

| #                 | GET                   | POST                  | PUT                   | DELETE                |
|:-----------------:|:---------------------:|:---------------------:|:---------------------:|:---------------------:|
| Warehouses        | :heavy_check_mark:    | :x:                   | :x:                   | :x:                   |
| Items             | :heavy_check_mark:    | :heavy_check_mark:    | :x:                   | :x:                   |
| WarehouseItems    | :heavy_check_mark:    | :heavy_check_mark:    | :heavy_check_mark:    | :heavy_check_mark:    |

# curl

Set corresponding variable for:
```sh
# docker and docker-compose
HOST=localhost:8080
# kubernetes (minikube) and helm
HOST=wms.com:80
```

## Warehouses
### GET
#### Success
```sh
curl -v -H "Content-Type: application/json" http://$HOST/api/warehouses
curl -v -H "Content-Type: application/json" http://$HOST/api/warehouses/1
```
#### Fail
```sh
curl -v -H "Content-Type: application/json" http://$HOST/api/warehouses/999
```

## Items
### GET
#### Success
```sh
curl -v -H "Content-Type: application/json" http://$HOST/api/items
curl -v -H "Content-Type: application/json" http://$HOST/api/items/1
```
#### Fail
```sh
curl -v -H "Content-Type: application/json" http://$HOST/api/items/999
```

### POST
#### Success
```sh
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/items -d @"payload/CreateItem.json"
```
#### Fail
```sh
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/items -d @"payload/CreateItemWithId.json"
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/items -d @"payload/CreateItemWithNegativePrice.json"
```

### PUT
#### Success
```sh
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/items -d @"payload/UpdateItem.json"
```
#### Fail
```sh
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/items -d @"payload/UpdateItemWithIncorrectId.json"
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/items -d @"payload/UpdateItemWithIncorrectData.json"
```

## WarehouseItems
### GET
#### Success
```sh
curl -v -H "Content-Type: application/json" http://$HOST/api/WarehouseItems
curl -v -H "Content-Type: application/json" http://$HOST/api/WarehouseItems/1
```
#### Fail
```sh
curl -v -H "Content-Type: application/json" http://$HOST/api/WarehouseItems/999
```

### POST
#### Success
```sh
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/warehouseitems -d @"payload/CreateWarehouseItem.json"
```
#### Fail
```sh
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/warehouseitems -d @"payload/CreateWarehouseItemWithId.json"
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/warehouseitems -d @"payload/CreateWarehouseItemWithNegativeCount.json"
```

### PUT
#### Success
```sh
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/warehouseitems -d @"payload/UpdateWarehouseItem.json"
```
#### Fail
```sh
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/warehouseitems -d @"payload/UpdateWarehouseItemWithIncorrectId.json"
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/warehouseitems -d @"payload/UpdateWarehouseItemWithIncorrectIds.json"
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/warehouseitems -d @"payload/UpdateWarehouseItemWithIncorrectData.json"
```

### DELETE
#### Success
```sh
curl -v -H "Content-Type: application/json" -X DELETE http://$HOST/api/warehouseitems/1
```
#### Fail
```sh
curl -v -H "Content-Type: application/json" -X DELETE http://$HOST/api/warehouseitems/999
```
