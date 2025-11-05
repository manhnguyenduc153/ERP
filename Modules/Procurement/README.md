# Procurement Module

## Mô tả

Module quản lý mua hàng và nhà cung cấp.

## Chức năng chính

- 📝 Purchase Order Management (CRUD)
- 🏭 Supplier Management
- 👤 Purchase Staff Management
- 📊 Purchase Analytics
- ✅ PO Approval Workflow

## Components

### Controllers

- **PurchaseOrdersController**: Quản lý đơn mua hàng
- **SupplierController**: Quản lý nhà cung cấp

### Services

- **PurchaseOrderService**: Business logic cho purchase orders
- **SupplierService**: Business logic cho suppliers
- **PurchaseStaffService**: Business logic cho purchase staff

### Repositories

- **PurchaseOrderRepository**: Data access cho purchase orders
- **SupplierRepository**: Data access cho suppliers
- **PurchaseStaffRepository**: Data access cho purchase staff

### DTOs

- PurchaseOrder DTOs (CreatePurchaseOrderDto, UpdatePurchaseOrderDto, PurchaseOrderResponseDto)
- PurchaseOrderMapper: Chuyển đổi Entity ↔️ DTO

## Dependencies

- Core.Common (Models, Enums)
- Core.Database (Entities, DbContext, BaseRepository)
- Inventory Module (Product information)

## Business Rules

- Purchase order phải có ít nhất 1 detail item
- Chỉ có thể edit/delete purchase order ở trạng thái Pending
- Approved purchase order sẽ tự động update inventory
