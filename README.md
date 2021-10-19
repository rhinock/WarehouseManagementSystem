# WarehouseManagementSystem

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
- docker, docker-compose
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

- db-data (data for postgres)
  - volume for docker-compose
  - for database recreating should be removed manually

# RESTful APIs

| #                 | GET                   | POST                  | PUT                   | DELETE                |
|:-----------------:|:---------------------:|:---------------------:|:---------------------:|:---------------------:|
| Warehouses        | :heavy_check_mark:    | :x:                   | :x:                   | :x:                   |
| Items             | :heavy_check_mark:    | :heavy_check_mark:    | :x:                   | :x:                   |
| WarehouseItems    | :heavy_check_mark:    | :heavy_check_mark:    | :heavy_check_mark:    | :heavy_check_mark:    |

# curl

Set corresponding variable:
```sh
# for docker
HOST=localhost:8080
# for minikube
HOST=wms.com:80
```

## Warehouses
### GET

- Success
```
curl -v -H "Content-Type: application/json" http://$HOST/api/warehouses
curl -v -H "Content-Type: application/json" http://$HOST/api/warehouses/1
```
- Fail
```
curl -v -H "Content-Type: application/json" http://$HOST/api/warehouses/999
```

## Items
### GET

- Success
```
curl -v -H "Content-Type: application/json" http://$HOST/api/items
curl -v -H "Content-Type: application/json" http://$HOST/api/items/1
```
- Fail
```
curl -v -H "Content-Type: application/json" http://$HOST/api/items/999
```

### POST

- Success
```
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/items -d @"payload/CreateItem.json"
```
- Fail
```
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/items -d @"payload/CreateItemWithId.json"
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/items -d @"payload/CreateItemWithNegativePrice.json"
```

### PUT

- Success
```
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/items -d @"payload/UpdateItem.json"
```
- Fail
```
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/items -d @"payload/UpdateItemWithIncorrectId.json"
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/items -d @"payload/UpdateItemWithIncorrectData.json"
```

## WarehouseItems
### GET

- Success
```
curl -v -H "Content-Type: application/json" http://$HOST/api/WarehouseItems
curl -v -H "Content-Type: application/json" http://$HOST/api/WarehouseItems/1
```
- Fail
```
curl -v -H "Content-Type: application/json" http://$HOST/api/WarehouseItems/999
```

### POST

- Success
```
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/warehouseitems -d @"payload/CreateWarehouseItem.json"
```
- Fail
```
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/warehouseitems -d @"payload/CreateWarehouseItemWithId.json"
curl -v -H "Content-Type: application/json" -X POST http://$HOST/api/warehouseitems -d @"payload/CreateWarehouseItemWithNegativeCount.json"
```

### PUT

- Success
```
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/warehouseitems -d @"payload/UpdateWarehouseItem.json"
```
- Fail
```
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/warehouseitems -d @"payload/UpdateWarehouseItemWithIncorrectId.json"
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/warehouseitems -d @"payload/UpdateWarehouseItemWithIncorrectIds.json"
curl -v -H "Content-Type: application/json" -X PUT http://$HOST/api/warehouseitems -d @"payload/UpdateWarehouseItemWithIncorrectData.json"
```

### DELETE

- Success
```
curl -v -H "Content-Type: application/json" -X DELETE http://$HOST/api/warehouseitems/1
```
- Fail
```
curl -v -H "Content-Type: application/json" -X DELETE http://$HOST/api/warehouseitems/999
```
