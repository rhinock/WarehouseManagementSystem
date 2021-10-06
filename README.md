# WarehouseManagementSystem

The system provides next functionality:
- view list of warehouses, items and warehouse items
- create a new item with particular price
- add, edit or delete an item to/from warehouse with particular amount

Note. Warehouses, Items and WarehouseItems -- are different tables.

Requirements:
- .NET Core
- EF Core
- Any DB (currently PostgreSQL)
- only server side (WebAPI controllers)
- integration tests (webhost, xUnit)
- OOP and SOLID principles

![](Diagram.png)

Initial Data

- Items

| Id | Name | Price |
|:--:|:----:|:-----:|
| 1 | pencil | 10.00
| 2 | pen | 20.00
| 3 | felt-tip pen | 30.00

- Warehouses

| Id | Name | MaximumItems |
|:--:|:----:|:------------:|
| 1 | miniature | 100
| 2 | �����  | 10000
| 3 | ����� ����������� | 1000000

- WarehouseItems

| Id | WarehouseId | ItemId | Count |
|:--:|:-----------:|:------:|:------:
| 1 | 1 | 1 | 50
| 2 | 2 | 2 | 5000
| 3 | 3 | 3 | 500000