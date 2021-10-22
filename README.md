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

# dotnet

```sh
dotnet run --project WMS.UI/WMS.UI.csproj
```

# RESTful APIs

| #                 | GET                   | POST                  | PUT                   | DELETE                |
|:-----------------:|:---------------------:|:---------------------:|:---------------------:|:---------------------:|
| Warehouses        | :heavy_check_mark:    | :x:                   | :x:                   | :x:                   |
| Items             | :heavy_check_mark:    | :heavy_check_mark:    | :x:                   | :x:                   |
| WarehouseItems    | :heavy_check_mark:    | :heavy_check_mark:    | :heavy_check_mark:    | :heavy_check_mark:    |

# Docker

- [Get Docker](https://docs.docker.com/get-docker/)
- [Dockerfile reference](https://docs.docker.com/engine/reference/builder/)
- [Dockerize an ASP.NET Core application](https://docs.docker.com/samples/dotnetcore/)
- [postgres - How to use this image](https://hub.docker.com/_/postgres)
- [Network settings](https://docs.docker.com/engine/reference/run/#network-settings)

## Build image and push to hub.docker.com

```sh
docker build -t wms:latest .
# docker tag <image>:<tag> <login>/<image>:<tag>
docker tag wms:latest rhinock/wms:latest
# docker push <login>/<image>:<tag>
docker push rhinock/wms:latest

docker image ls -a
```

### Remove Dockerfile generated items 

```sh
docker image rm rhinock/wms:latest
docker image rm wms:latest
docker image rm mcr.microsoft.com/dotnet/sdk:3.1-alpine
docker image rm mcr.microsoft.com/dotnet/aspnet:3.1-alpine
docker image prune -f
docker volume prune -f

docker image ls -a
docker volume ls
```

## Run application with bridge network driver

```sh
docker network create app --driver bridge

docker run --name postgres -e POSTGRES_PASSWORD=1234 -e POSTGRES_DB=WMS -d --network=app postgres:12-alpine

docker run --name wms -e ConnectionStrings__WmsDbContextPostgres="Host=postgres;Port=5432;Database=WMS;Username=postgres;Password=1234" -d --network=app -p 8080:80 rhinock/wms:latest

docker network ls
docker image ls -a
docker container ls -a
```

### Remove Dockerfile generated items 

```sh
docker container rm -f wms
docker container rm -f postgres
docker image rm postgres:12-alpine
docker image rm rhinock/wms:latest
docker network rm app
docker volume prune -f

docker network ls
docker image ls -a
docker volume ls
docker container ls -a
```

## Run application without bridge network driver

```docker
docker run --name postgres -e POSTGRES_PASSWORD=1234 -e POSTGRES_DB=WMS -d postgres:12-alpine

docker run --name wms -e ConnectionStrings__WmsDbContextPostgres="Host=postgres;Port=5432;Database=WMS;Username=postgres;Password=1234" -d -p 8080:80 --link postgres:postgres rhinock/wms:latest

docker image ls -a
docker container ls -a
```

## env variables

```sh
docker exec postgres env | grep 'POSTGRES_PASSWORD\|POSTGRES_DB'
docker exec wms env | grep ConnectionStrings__WmsDbContextPostgres
```

## psql

```sh
docker exec -it postgres psql -U postgres
```

```psql
# list databases
\l
# connect to database 'WMS'
\c WMS
# list all schemas
\dn
# list all tables in schema 'public'
\dt public.*
# select data from all tables
SELECT * FROM public."Items";
SELECT * FROM public."Warehouses";
SELECT * FROM public."WarehouseItems";
# delete non-initial data
DELETE FROM public."Items" WHERE "Id">3;
DELETE FROM public."WarehouseItems" WHERE "Id">3;
```

## Remove Dockerfile generated items

```sh
docker container rm -f wms
docker container rm -f postgres
docker image rm postgres:12-alpine
docker image rm rhinock/wms:latest
docker volume prune -f

docker image ls -a
docker volume ls
docker container ls -a
```

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

## Build images locally

```sh
docker-compose up -d

docker network ls
docker image ls -a
docker container ls -a
```

### Remove Docker Compose generated items

```sh
docker-compose down
docker image rm rhinock/wms:latest
docker image rm postgres:12-alpine
docker image rm mcr.microsoft.com/dotnet/sdk:3.1-alpine
docker image rm mcr.microsoft.com/dotnet/aspnet:3.1-alpine
docker image prune -f
docker volume prune -f

docker network ls
docker image ls -a
docker volume ls
docker container ls -a
```

## Pull images from hub.docker.com

```sh
docker-compose pull
docker-compose up -d

docker network ls
docker image ls -a
docker volume ls
docker container ls -a
```

### Remove Docker Compose generated items

```sh
docker-compose down
docker image rm postgres:12-alpine
docker image rm rhinock/wms:latest
docker volume prune -f

docker network ls
docker volume ls
docker image ls -a
docker container ls -a
```

# Kubernetes (minikube)

- [minikube start](https://minikube.sigs.k8s.io/docs/start/)
- [kubectl Cheat Sheet](https://kubernetes.io/docs/reference/kubectl/cheatsheet/)
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

## Apply manifests

```sh
k apply -f k8s/
```

## Remove Kubernetes generated items

```sh
k delete -f k8s/
k delete pvc pg-data-postgres-statefulset-0
```

## Get Kubernetes generated items

```sh
k get sc
k get pv
k get pvc
k get sts
k get deploy
k get rs
k get svc
k get ingress
k get endpoints
k get po
```

# Helm

- [Installing Helm](https://helm.sh/docs/intro/install/)
- [Getting Started](https://helm.sh/docs/chart_template_guide/getting_started/)
- [Helm Create](https://helm.sh/docs/helm/helm_create/)
- [Helm Template](https://helm.sh/docs/helm/helm_template/)
- [Built-in Objects](https://helm.sh/docs/chart_template_guide/builtin_objects/)
  - `{{ .Chart.Name }}`
- [Values not exist with loop over a range](https://github.com/helm/helm/issues/1311#issuecomment-252536380)
  - `{{- $root := . -}}`
  - `{{ $root.Values.service.port }}`
- [Kubernetes Helm, combine two variables with a string in the middle](https://stackoverflow.com/a/62874276/16420746)
  - `"{{ $root.Chart.Name }}-service"`    
- [Helm Install](https://helm.sh/docs/helm/helm_install/)
- [Helm Get Manifest](https://helm.sh/docs/helm/helm_get_manifest/)
- [Helm Upgrade](https://helm.sh/docs/helm/helm_upgrade/)
- [Helm History](https://helm.sh/docs/helm/helm_history/)
- [Helm Rollback](https://helm.sh/docs/helm/helm_rollback/)
- [Helm Uninstall](https://helm.sh/docs/helm/helm_uninstall/)

## Generate helm templates from chart

```sh
helm template chart
```

## Create helm chart

```sh
helm create chart
```

## Compress helm chart

```sh
tar -cvzf chart.tgz chart
```

Output:
```
chart/
chart/Chart.yaml
chart/values.yaml
chart/.helmignore
chart/templates/
chart/templates/ingress.yaml
chart/templates/wms-service.yml
chart/templates/wms-deployment.yml
chart/templates/postgres-statefulset.yaml
chart/templates/postgres-service.yaml
chart/templates/storageClass.yaml
```

## Install helm chart from archive

```sh
helm install wms chart.tgz
```

## Install helm chart from folder

```sh
helm install wms chart/
```

## Upgrade helm chart

```sh
helm upgrade wms chart/
```

## Get history of helm charts

```sh
helm history wms
```

## Rollback helm chart

```sh
helm rollback wms 1
```

## Uninstall helm chart

```sh
helm uninstall wms
k delete pvc pg-data-wms-postgres-statefulset-0
```

## Get helm items

```sh
helm list
helm get manifest wms
helm get manifest wms --revision 1
helm get manifest wms --revision 2
```

## Get kubernetes items generated by helm

```sh
k get sc
k get pv
k get pvc
k get sts
k get deploy
k get rs
k get svc
k get ingress
k get endpoints
k get po
```

# curl

Set corresponding variable for:
```sh
# local deployment
HOST=localhost:5000
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
