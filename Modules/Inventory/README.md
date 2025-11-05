# Inventory Module

## Mô tả

Module quản lý kho hàng, sản phẩm và danh mục sản phẩm.

## Chức năng chính

- 📦 Product Management (CRUD)
- 🏷️ Category Management
- 🏪 Warehouse Management
- 📊 Stock Transaction Tracking
- 📈 Warehouse Reports & Analytics
- 🏬 Store Management

## Components

### Controllers

- **ProductsController**: Quản lý sản phẩm
- **CategoryController**: Quản lý danh mục
- **WarehouseController**: Quản lý kho hàng
- **WarehouseReportController**: Báo cáo kho hàng

### Services

- **ProductService**: Business logic cho products
- **CategoryService**: Business logic cho categories
- **WarehouseService**: Business logic cho warehouses
- **WarehouseReportService**: Business logic cho reports
- **StockTransactionService**: Quản lý giao dịch kho
- **StoreService**: Quản lý cửa hàng

### Repositories

- **CategoryRepository**: Data access cho categories
- **WarehouseRepository**: Data access cho warehouses
- **WarehouseReportRepository**: Data access cho reports
- **StockTransactionRepository**: Data access cho transactions
- **StoreRepository**: Data access cho stores

### DTOs

- Product DTOs (CreateProductDto, UpdateProductDto, ProductResponseDto)
- ProductMapper: Chuyển đổi Entity ↔️ DTO

## Dependencies

- Core.Common (Models, Enums)
- Core.Database (Entities, DbContext, BaseRepository)
