<h1 align=center>WarehouseManagementSystem</h1>

The system provides next functionality:
- view list of warehouses, items and warehouse items
- add or edit an item with particular price
- add, edit or delete an item to/from warehouse with particular amount

**Note**. Warehouses, Items and WarehouseItems -- are different tables.

Requirements:
- .NET Core
- EF Core
- PostgreSQL
- only server side (WebAPI controllers)
- integration tests (webhost, xUnit)
- OOP and SOLID principles

<h1 align=center>PostgreSQL</h1>
<h2 align=center>Diagram</h1>

<p align="center">
 <img src="Diagram.png"/>
</p>

<h2 align=center>Initial Data</h1>

<table>

 <tr>
  <th>#</th>
  <th>Items</th>
  <th>Warehouses</th>
  <th>WarehouseItems</th>
 </tr>

 <tr>

  <td>

| Id |
|:--:|
| 1 | 
| 2 | 
| 3 | 

  </td>
  <td>

| Name | Price |
|:----:|:-----:|
| pencil | 10.00
| pen | 20.00
| felt-tip pen | 30.00

  </td>
  <td>

| Name | MaximumItems |
|:----:|:------------:|
| miniature | 100
| decent | 10000
| hefty | 1000000

  </td>
  <td>

| WarehouseId | ItemId | Count |
|:-----------:|:------:|:-----:|
| 1 | 1 | 50
| 2 | 2 | 5000
| 3 | 3 | 500000

  </td>
 </tr> 

</table>

<h1 align=center>Docker</h1>

- db-data
  - volume for docker-compose
  - for database recreating should be removed manually

<h1 align=center>curl</h1>
<h2 align=center>Warehouses</h1>
<h3 align=center>GET</h1>

```
curl -i -H "Content-Type: application/json" -X GET http://localhost:8080/api/warehouses
curl -i -H "Content-Type: application/json" -X GET http://localhost:8080/api/items/1
curl -i -H "Content-Type: application/json" -X GET http://localhost:8080/api/items/999
```

<h2 align=center>Items</h1>
<h3 align=center>GET</h1>

```
curl -i -H "Content-Type: application/json" -X GET http://localhost:8080/api/items
curl -i -H "Content-Type: application/json" -X GET http://localhost:8080/api/items/1
```

<h3 align=center>POST</h1>

```
curl -i -H "Content-Type: application/json" -X POST http://localhost:8080/api/items -d @"payload/CreateItem.json"
curl -i -H "Content-Type: application/json" -X POST http://localhost:8080/api/items -d @"payload/CreateItemWithId.json"
curl -i -H "Content-Type: application/json" -X POST http://localhost:8080/api/items -d @"payload/CreateItemWithNegativePrice.json"
```

<h3 align=center>PUT</h1>

```
curl -i -H "Content-Type: application/json" -X PUT http://localhost:8080/api/items -d @"payload/UpdateItem.json"
curl -i -H "Content-Type: application/json" -X PUT http://localhost:8080/api/items -d @"payload/UpdateItemWithIncorrectId.json"
curl -i -H "Content-Type: application/json" -X PUT http://localhost:8080/api/items -d @"payload/UpdateItemWithIncorrectData.json"
```

<h2 align=center>WarehouseItems</h1>
<h3 align=center>GET</h1>

```
curl -i -H "Content-Type: application/json" -X GET http://localhost:8080/api/WarehouseItems
curl -i -H "Content-Type: application/json" -X GET http://localhost:8080/api/WarehouseItems/1
```

<h3 align=center>POST</h1>

```
curl -i -H "Content-Type: application/json" -X POST http://localhost:8080/api/warehouseitems -d @"payload/CreateWarehouseItem.json"
curl -i -H "Content-Type: application/json" -X POST http://localhost:8080/api/warehouseitems -d @"payload/CreateWarehouseItemWithId.json"
curl -i -H "Content-Type: application/json" -X POST http://localhost:8080/api/warehouseitems -d @"payload/CreateWarehouseItemWithNegativeCount.json"
```

<h3 align=center>PUT</h1>

```
curl -i -H "Content-Type: application/json" -X PUT http://localhost:8080/api/warehouseitems -d @"payload/UpdateWarehouseItem.json"
curl -i -H "Content-Type: application/json" -X PUT http://localhost:8080/api/warehouseitems -d @"payload/UpdateWarehouseItemWithIncorrectId.json"
curl -i -H "Content-Type: application/json" -X PUT http://localhost:8080/api/warehouseitems -d @"payload/UpdateWarehouseItemWithIncorrectIds.json"
curl -i -H "Content-Type: application/json" -X PUT http://localhost:8080/api/warehouseitems -d @"payload/UpdateWarehouseItemWithIncorrectData.json"
```

<h3 align=center>DELETE</h1>

```
curl -i -H "Content-Type: application/json" -X DELETE http://localhost:8080/api/warehouseitems/1
curl -i -H "Content-Type: application/json" -X DELETE http://localhost:8080/api/warehouseitems/999
```