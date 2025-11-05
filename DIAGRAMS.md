# ERP System - Component Diagram

## High-Level Architecture

```
╔═══════════════════════════════════════════════════════════════════╗
║                      ERP SYSTEM ARCHITECTURE                      ║
╠═══════════════════════════════════════════════════════════════════╣
║                                                                   ║
║  ┌─────────────────────  PRESENTATION LAYER  ─────────────────┐  ║
║  │                                                              │  ║
║  │   Swagger UI          REST APIs          Mobile/Web Client  │  ║
║  │                                                              │  ║
║  └──────────────────────────────┬───────────────────────────────┘  ║
║                                 │                                  ║
║  ┌──────────────────────  API CONTROLLERS  ────────────────────┐  ║
║  │                                                              │  ║
║  │  AccountsController  ProductsController  SalesController    │  ║
║  │  RoleController      WarehouseController PurchaseController │  ║
║  │  EmployeeController  CustomerController  DepartmentController│ ║
║  │                                                              │  ║
║  └──────────────────────────────┬───────────────────────────────┘  ║
║                                 │                                  ║
║  ┌──────────────────  BUSINESS COMPONENTS  ────────────────────┐  ║
║  │                                                              │  ║
║  │  ┌──────────┐  ┌───────────┐  ┌────────────┐  ┌─────────┐ │  ║
║  │  │ Identity │  │ Inventory │  │Procurement │  │  Sales  │ │  ║
║  │  │  Module  │  │  Module   │  │   Module   │  │ Module  │ │  ║
║  │  │          │  │           │  │            │  │         │ │  ║
║  │  │ • Auth   │  │ • Product │  │ • PO       │  │ • SO    │ │  ║
║  │  │ • Role   │  │ • Category│  │ • Supplier │  │ • Cust  │ │  ║
║  │  │ • User   │  │ • Warehouse│ │ • PO Staff│  │ • Staff │ │  ║
║  │  └────┬─────┘  └─────┬─────┘  └──────┬─────┘  └────┬────┘ │  ║
║  │       │              │                │             │      │  ║
║  │  ┌────┴────┐    ┌────┴─────┐                              │  ║
║  │  │   HR    │    │Reporting │                              │  ║
║  │  │ Module  │    │  Module  │                              │  ║
║  │  │         │    │          │                              │  ║
║  │  │ • Emp   │    │ • Reports│                              │  ║
║  │  │ • Dept  │    │ • KPI    │                              │  ║
║  │  └────┬────┘    └────┬─────┘                              │  ║
║  │       │              │                                     │  ║
║  └───────┼──────────────┼─────────────────────────────────────┘  ║
║          │              │                                        ║
║  ┌───────┴──────────────┴────────  CORE COMPONENT  ─────────┐   ║
║  │                                                            │   ║
║  │  ┌──────────────┐  ┌─────────────┐  ┌─────────────────┐ │   ║
║  │  │    Common    │  │   Database  │  │ Infrastructure  │ │   ║
║  │  │              │  │             │  │                 │ │   ║
║  │  │ • Models     │  │ • Entities  │  │ • Authorization │ │   ║
║  │  │ • Enums      │  │ • DbContext │  │ • Logging       │ │   ║
║  │  │ • Utilities  │  │ • BaseRepo  │  │ • Middleware    │ │   ║
║  │  │ • DTOs       │  │ • UnitOfWork│  │                 │ │   ║
║  │  └──────────────┘  └──────┬──────┘  └─────────────────┘ │   ║
║  │                           │                              │   ║
║  └───────────────────────────┼──────────────────────────────┘   ║
║                              │                                   ║
║  ┌──────────────────────  DATABASE LAYER  ────────────────────┐ ║
║  │                                                             │ ║
║  │                    ┌─────────────────┐                     │ ║
║  │                    │   MySQL Server  │                     │ ║
║  │                    │                 │                     │ ║
║  │                    │   erp_database  │                     │ ║
║  │                    └─────────────────┘                     │ ║
║  │                                                             │ ║
║  └─────────────────────────────────────────────────────────────┘ ║
║                                                                   ║
╚═══════════════════════════════════════════════════════════════════╝
```

## Module Communication Flow

```
┌────────────┐
│   Client   │
└──────┬─────┘
       │ HTTP Request
       ▼
┌────────────────────┐
│  Sales Controller  │
└─────────┬──────────┘
          │
          ▼
┌────────────────────┐
│  Sales Service     │────────┐
└─────────┬──────────┘        │
          │                   │ Need to check stock
          │                   │
          ▼                   ▼
┌────────────────────┐  ┌──────────────────┐
│ Sales Repository   │  │Inventory Service │
└─────────┬──────────┘  └────────┬─────────┘
          │                      │
          ▼                      ▼
┌────────────────────┐  ┌──────────────────┐
│   UnitOfWork       │  │ Product Repository│
└─────────┬──────────┘  └────────┬─────────┘
          │                      │
          └──────────┬───────────┘
                     │
                     ▼
            ┌────────────────┐
            │   DbContext    │
            └────────┬───────┘
                     │
                     ▼
            ┌────────────────┐
            │    Database    │
            └────────────────┘
```

## Dependency Graph

```
                    ┌───────────────┐
                    │   Reporting   │
                    │    Module     │
                    └───────┬───────┘
                            │ depends on
            ┌───────────────┼───────────────┐
            │               │               │
    ┌───────▼──────┐ ┌──────▼──────┐ ┌─────▼──────┐
    │   Identity   │ │  Inventory  │ │   Sales    │
    │    Module    │ │    Module   │ │   Module   │
    └───────┬──────┘ └──────┬──────┘ └─────┬──────┘
            │               │               │
            │          ┌────▼────┐          │
            │          │Procure  │          │
            │          │ment     │          │
            │          │Module   │          │
            │          └────┬────┘          │
            │               │               │
            └───────────────┼───────────────┘
                            │ ALL depend on
                    ┌───────▼───────┐
                    │     CORE      │
                    │   Component   │
                    └───────┬───────┘
                            │
                    ┌───────▼───────┐
                    │   Database    │
                    └───────────────┘
```

## Component Layers (Example: Sales Module)

```
┌─────────────────────────────────────────────────────┐
│              SALES MODULE                           │
├─────────────────────────────────────────────────────┤
│                                                     │
│  ┌────────────────────────────────────────────┐   │
│  │  Controllers (API Layer)                   │   │
│  │  ┌──────────────────┐ ┌─────────────────┐ │   │
│  │  │SalesOrders       │ │ Customer        │ │   │
│  │  │Controller        │ │ Controller      │ │   │
│  │  └────────┬─────────┘ └────────┬────────┘ │   │
│  └───────────┼────────────────────┼──────────┘   │
│              │                    │               │
│  ┌───────────▼────────────────────▼──────────┐   │
│  │  Services (Business Logic Layer)          │   │
│  │  ┌──────────────────┐ ┌─────────────────┐ │   │
│  │  │SalesOrderService │ │ CustomerService │ │   │
│  │  └────────┬─────────┘ └────────┬────────┘ │   │
│  └───────────┼────────────────────┼──────────┘   │
│              │                    │               │
│  ┌───────────▼────────────────────▼──────────┐   │
│  │  Repositories (Data Access Layer)         │   │
│  │  ┌──────────────────┐ ┌─────────────────┐ │   │
│  │  │SalesOrderRepo    │ │ CustomerRepo    │ │   │
│  │  └────────┬─────────┘ └────────┬────────┘ │   │
│  └───────────┼────────────────────┼──────────┘   │
│              │                    │               │
│  ┌───────────▼────────────────────▼──────────┐   │
│  │  DTOs & Mappers                           │   │
│  │  ┌──────────────────┐ ┌─────────────────┐ │   │
│  │  │SalesOrderDto     │ │ CustomerDto     │ │   │
│  │  │SalesOrderMapper  │ │ CustomerMapper  │ │   │
│  │  └──────────────────┘ └─────────────────┘ │   │
│  └───────────────────────────────────────────┘   │
│                                                   │
└───────────────────────┬───────────────────────────┘
                        │ uses
                        ▼
        ┌───────────────────────────────┐
        │    CORE.DATABASE              │
        │  ┌─────────────────────────┐  │
        │  │  ErpDbContext           │  │
        │  │  BaseRepository<T>      │  │
        │  │  UnitOfWork             │  │
        │  └─────────────────────────┘  │
        └───────────────────────────────┘
```

## Data Flow Sequence

```
1. CREATE SALES ORDER
════════════════════

Client                Controller          Service             Repository         Database
  │                       │                   │                    │                │
  ├─POST /api/orders─────▶│                   │                    │                │
  │                       │                   │                    │                │
  │                       ├─CreateAsync()────▶│                    │                │
  │                       │                   │                    │                │
  │                       │                   ├─Validate()         │                │
  │                       │                   │                    │                │
  │                       │                   ├─CheckStock()       │                │
  │                       │                   │  (call Inventory)  │                │
  │                       │                   │                    │                │
  │                       │                   ├─AddAsync()────────▶│                │
  │                       │                   │                    │                │
  │                       │                   │                    ├─INSERT────────▶│
  │                       │                   │                    │                │
  │                       │                   │                    │◀─Success───────┤
  │                       │                   │◀──Result───────────┤                │
  │                       │                   │                    │                │
  │                       │                   ├─DecreaseStock()    │                │
  │                       │                   │  (call Inventory)  │                │
  │                       │                   │                    │                │
  │                       │◀──Response────────┤                    │                │
  │                       │                   │                    │                │
  │◀─200 OK───────────────┤                   │                    │                │
  │                       │                   │                    │                │
```

## Cross-Module Communication

```
┌─────────────────────────────────────────────────────────────────┐
│  Scenario: Create Sales Order with Stock Check                 │
└─────────────────────────────────────────────────────────────────┘

    ┌──────────────┐
    │  Sales Module│
    │              │
    │ ┌──────────┐ │
    │ │ Service  │ │
    │ └────┬─────┘ │
    └──────┼───────┘
           │
           │ 1. Need to check if product in stock
           │
           ▼
    ┌──────────────────┐
    │ Inventory Module │
    │                  │
    │ ┌──────────────┐ │
    │ │ProductService│ │
    │ └──────┬───────┘ │
    │        │         │
    │ ┌──────▼───────┐ │
    │ │  Repository  │ │
    │ └──────┬───────┘ │
    └────────┼─────────┘
             │
             │ 2. Query database for stock
             │
             ▼
    ┌────────────────┐
    │   Database     │
    │                │
    │  Products      │
    │  Inventory     │
    └────────────────┘
```

## Future Evolution Path

```
PHASE 1: Current State
══════════════════════
┌──────────────────────────────┐
│     Single Application       │
│  ┌────────────────────────┐  │
│  │ All Modules Together   │  │
│  └────────────────────────┘  │
│  ┌────────────────────────┐  │
│  │   Shared Database      │  │
│  └────────────────────────┘  │
└──────────────────────────────┘


PHASE 2: Service-Oriented
══════════════════════════
         ┌──────────┐
         │   API    │
         │ Gateway  │
         └────┬─────┘
              │
      ┌───────┼────────┐
      │       │        │
┌─────▼──┐ ┌──▼───┐ ┌─▼────┐
│Identity│ │Sales │ │Invent│
│Service │ │Service│ │ory   │
└────┬───┘ └──┬───┘ └─┬────┘
     └────────┼───────┘
         ┌────▼────┐
         │Shared DB│
         └─────────┘


PHASE 3: Microservices
══════════════════════
         ┌──────────┐
         │   API    │
         │ Gateway  │
         └────┬─────┘
              │
      ┌───────┼────────┐
      │       │        │
┌─────▼──┐ ┌──▼───┐ ┌─▼────┐
│Identity│ │Sales │ │Invent│
│Service │ │Service│ │ory   │
└────┬───┘ └──┬───┘ └─┬────┘
┌────▼───┐ ┌──▼───┐ ┌─▼────┐
│  DB1   │ │  DB2 │ │  DB3 │
└────────┘ └──────┘ └──────┘
```

## Request Processing Pipeline

```
┌────────────────────────────────────────────────────────┐
│             HTTP REQUEST PIPELINE                      │
└────────────────────────────────────────────────────────┘

HTTP Request
     │
     ▼
┌─────────────────┐
│  Middleware     │
│  • Exception    │
│  • Logging      │
│  • CORS         │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Authentication  │
│ • Cookie        │
│ • Validate      │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Authorization   │
│ • Permission    │
│ • Check Role    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   Controller    │
│ • Parse Request │
│ • Validate DTO  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│    Service      │
│ • Business Logic│
│ • Validation    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   Repository    │
│ • Data Access   │
│ • CRUD Ops      │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   Database      │
│ • Execute Query │
│ • Return Data   │
└────────┬────────┘
         │
         ▼
HTTP Response (JSON)
```

## Component Registry

```
╔═══════════════════════════════════════════════════════╗
║              COMPONENT REGISTRY                       ║
╠═══════════════════════════════════════════════════════╣
║                                                       ║
║  Component Name: Identity                            ║
║  ├─ Version: 1.0                                     ║
║  ├─ Owner: Team Identity                             ║
║  ├─ Public APIs:                                     ║
║  │  • POST /api/accounts/login                       ║
║  │  • POST /api/accounts/register                    ║
║  │  • GET  /api/roles                                ║
║  └─ Dependencies: Core.Common, Core.Database         ║
║                                                       ║
║  Component Name: Inventory                           ║
║  ├─ Version: 1.0                                     ║
║  ├─ Owner: Team Inventory                            ║
║  ├─ Public APIs:                                     ║
║  │  • GET    /api/products                           ║
║  │  • POST   /api/products                           ║
║  │  • PUT    /api/products/{id}                      ║
║  │  • DELETE /api/products/{id}                      ║
║  │  • GET    /api/warehouses                         ║
║  └─ Dependencies: Core.Common, Core.Database         ║
║                                                       ║
║  Component Name: Sales                               ║
║  ├─ Version: 1.0                                     ║
║  ├─ Owner: Team Sales                                ║
║  ├─ Public APIs:                                     ║
║  │  • GET  /api/sales-orders                         ║
║  │  • POST /api/sales-orders                         ║
║  └─ Dependencies: Core, Inventory (stock check)      ║
║                                                       ║
╚═══════════════════════════════════════════════════════╝
```
