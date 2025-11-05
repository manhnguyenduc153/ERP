# Sales Module

## Mô tả

Module quản lý bán hàng và khách hàng.

## Chức năng chính

- 🛒 Sales Order Management (CRUD)
- 👥 Customer Management
- 👤 Sales Staff Management
- 💰 Revenue Tracking
- 📊 Sales Analytics

## Components

### Controllers

- **SalesOrdersController**: Quản lý đơn bán hàng
- **CustomerController**: Quản lý khách hàng

### Services

- **SalesOrderService**: Business logic cho sales orders
- **CustomerService**: Business logic cho customers
- **SaleStaffService**: Business logic cho sales staff

### Repositories

- **SalesOrderRepository**: Data access cho sales orders
- **CustomerRepository**: Data access cho customers
- **SaleStaffRepository**: Data access cho sales staff

### DTOs

- SalesOrder DTOs (CreateSalesOrderDto, UpdateSalesOrderDto, SalesOrderResponseDto)
- SalesOrderMapper: Chuyển đổi Entity ↔️ DTO

## Dependencies

- Core.Common (Models, Enums)
- Core.Database (Entities, DbContext, BaseRepository)
- Inventory Module (Product information, Stock checking)

## Business Rules

- Sales order phải có ít nhất 1 detail item
- Phải check inventory trước khi tạo sales order
- Completed sales order sẽ tự động giảm inventory
- Sales order có thể cancel nếu chưa shipped
