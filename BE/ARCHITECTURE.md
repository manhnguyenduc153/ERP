# ERP System - Component-Based Architecture

## 🏗️ Kiến trúc

Project này được thiết kế theo **Component-Based Architecture** (CBA), trong đó hệ thống được chia thành các business components độc lập, dễ bảo trì và mở rộng.

## 📁 Cấu trúc Project

```
ERP-API/
├── Core/                           # Shared Components
│   ├── Common/                     # Models, Enums, Utilities dùng chung
│   │   ├── Models/                 # ResponseData, PageList, BaseSearch
│   │   ├── Enums/                  # ErrorCodeAPI, PermissionEnum
│   │   └── Utilities/              # Extension methods, Helpers
│   ├── Database/                   # Database infrastructure
│   │   ├── Entities/               # Domain entities
│   │   ├── ErpDbContext.cs         # EF Core DbContext
│   │   ├── BaseRepository.cs       # Generic repository
│   │   └── UnitOfWork.cs           # Unit of Work pattern
│   └── Infrastructure/             # Cross-cutting concerns
│       ├── HasPermissionAttribute.cs
│       └── PermissionAuthorization.cs
│
├── Modules/                        # Business Components
│   ├── Identity/                   # Authentication & Authorization
│   │   ├── Controllers/
│   │   ├── Services/
│   │   └── DTOs/
│   │
│   ├── Inventory/                  # Product & Warehouse Management
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Repositories/
│   │   └── DTOs/
│   │
│   ├── Procurement/                # Purchase Orders & Suppliers
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Repositories/
│   │   ├── Mappers/
│   │   └── DTOs/
│   │
│   ├── Sales/                      # Sales Orders & Customers
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Repositories/
│   │   ├── Mappers/
│   │   └── DTOs/
│   │
│   ├── HumanResources/             # Employee & Department Management
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Repositories/
│   │   └── DTOs/
│   │
│   └── Reporting/                  # Analytics & Reports
│       ├── Controllers/
│       └── Services/
│
├── Migrations/                     # EF Core Migrations
├── Extensions/                     # Service Registration Extensions
├── Program.cs                      # Application entry point
└── appsettings.json               # Configuration
```

## 🎯 Nguyên tắc thiết kế

### 1. **Separation of Concerns**

Mỗi module tập trung vào một business domain cụ thể:

- ✅ Identity: Authentication & Authorization
- ✅ Inventory: Product & Warehouse
- ✅ Procurement: Purchasing
- ✅ Sales: Selling
- ✅ HumanResources: HR Management
- ✅ Reporting: Analytics

### 2. **Dependency Rule**

```
Modules → Core (ALLOWED)
Core → Modules (FORBIDDEN)
Module A → Module B (MINIMIZE)
```

- **Core** không phụ thuộc vào bất kỳ Module nào
- **Modules** chỉ phụ thuộc vào Core và tối thiểu hóa dependencies giữa các modules
- Communication giữa modules thông qua interfaces/events (future)

### 3. **Layered Architecture trong mỗi Module**

```
Controllers (API Layer)
    ↓
Services (Business Logic Layer)
    ↓
Repositories (Data Access Layer)
    ↓
Core.Database (Database Layer)
```

### 4. **SOLID Principles**

- **S**: Single Responsibility - Mỗi class có 1 nhiệm vụ duy nhất
- **O**: Open/Closed - Mở để mở rộng, đóng để sửa đổi
- **L**: Liskov Substitution - Có thể thay thế bằng subtype
- **I**: Interface Segregation - Interface nhỏ và cụ thể
- **D**: Dependency Inversion - Phụ thuộc vào abstraction

## 🔄 Data Flow

```
Client Request
    ↓
[Controller] - Receive HTTP request
    ↓
[Service] - Business logic validation
    ↓
[Repository] - Data access via BaseRepository
    ↓
[DbContext] - EF Core query execution
    ↓
[Database] - MySQL
    ↓
[Response] - DTO mapping & return
```

## 📦 Module Dependencies

```
Identity
  └── Core (Common, Database, Infrastructure)

Inventory
  └── Core (Common, Database)

Procurement
  ├── Core (Common, Database)
  └── Inventory (Product info) [optional]

Sales
  ├── Core (Common, Database)
  └── Inventory (Stock checking) [optional]

HumanResources
  ├── Core (Common, Database)
  └── Identity (User linking) [optional]

Reporting
  ├── Core (Common, Database)
  └── All Modules (Data aggregation)
```

## 🚀 Lợi ích của Component-Based Architecture

### 1. **Modularity** (Tính mô-đun)

- Mỗi component là một đơn vị độc lập
- Dễ dàng thêm/xóa modules mà không ảnh hưởng toàn bộ hệ thống

### 2. **Maintainability** (Dễ bảo trì)

- Code được tổ chức rõ ràng theo business domain
- Dễ tìm và fix bugs
- Giảm coupling giữa các phần của hệ thống

### 3. **Scalability** (Khả năng mở rộng)

- Có thể scale từng module riêng biệt
- Dễ dàng chuyển sang Microservices sau này
- Team có thể làm việc parallel trên các modules khác nhau

### 4. **Reusability** (Tái sử dụng)

- Core components có thể reuse cho nhiều modules
- DTOs và Models được share hiệu quả
- Repository pattern giảm duplicate code

### 5. **Testability** (Dễ test)

- Unit test từng module độc lập
- Mock dependencies dễ dàng
- Integration test theo business flow

### 6. **Team Organization** (Tổ chức team)

```
Team Identity    → Identity Module
Team Inventory   → Inventory Module
Team Procurement → Procurement Module
Team Sales       → Sales Module
Team HR          → HumanResources Module
```

## � Authentication & Authorization

### Cookie-based Authentication

Hệ thống sử dụng **Cookie-based Authentication** với ASP.NET Core Identity:

- **Cookie Name**: `.AspNetCore.Identity.Application`
- **Expiration**: 7 days (sliding)
- **Security**: HttpOnly, Secure, SameSite=Lax
- **Session**: Server-side session management

### Usage

```http
POST /api/accounts/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin@123"
}

Response: Set-Cookie: .AspNetCore.Identity.Application=...
```

Sau khi login, cookie tự động được gửi với mọi request.

## 📊 Chi tiết Components

### Core Component

**Mô tả**: Shared infrastructure cho toàn bộ hệ thống

**Chức năng**:

- Common models và utilities
- Database context và entities
- Base repository và Unit of Work
- Authorization infrastructure

**Dependencies**: Không phụ thuộc module nào

📄 **Chi tiết**: Xem `Core/README.md`

---

### Identity Module

**Mô tả**: Quản lý Authentication & Authorization

**Chức năng**:

- User login/logout
- Role management
- Permission control
- Cookie-based session

**API Endpoints**:

- `POST /api/accounts/login` - Login
- `POST /api/accounts/logout` - Logout
- `GET /api/roles` - Get roles

**Dependencies**: Core

📄 **Chi tiết**: Xem `Modules/Identity/README.md`

---

### Inventory Module

**Mô tả**: Quản lý kho hàng & sản phẩm

**Chức năng**:

- Product CRUD
- Category management
- Warehouse management
- Stock tracking

**API Endpoints**:

- `GET /api/products` - List products
- `POST /api/products` - Create product
- `GET /api/warehouses` - List warehouses

**Dependencies**: Core

📄 **Chi tiết**: Xem `Modules/Inventory/README.md`

---

### Procurement Module

**Mô tả**: Quản lý mua hàng & nhà cung cấp

**Chức năng**:

- Purchase Order CRUD
- Supplier management
- PO approval workflow

**API Endpoints**:

- `GET /api/purchase-orders` - List POs
- `POST /api/purchase-orders` - Create PO
- `GET /api/suppliers` - List suppliers

**Dependencies**: Core, Inventory (optional)

📄 **Chi tiết**: Xem `Modules/Procurement/README.md`

---

### Sales Module

**Mô tả**: Quản lý bán hàng & khách hàng

**Chức năng**:

- Sales Order CRUD
- Customer management
- Order fulfillment
- Stock checking

**API Endpoints**:

- `GET /api/sales-orders` - List orders
- `POST /api/sales-orders` - Create order
- `GET /api/customers` - List customers

**Dependencies**: Core, Inventory (optional)

� **Chi tiết**: Xem `Modules/Sales/README.md`

---

### HumanResources Module

**Mô tả**: Quản lý nhân sự & tổ chức

**Chức năng**:

- Employee management
- Department management
- Payroll (future)

**API Endpoints**:

- `GET /api/employees` - List employees
- `POST /api/employees` - Create employee
- `GET /api/departments` - List departments

**Dependencies**: Core, Identity (optional)

📄 **Chi tiết**: Xem `Modules/HumanResources/README.md`

---

### Reporting Module

**Mô tả**: Báo cáo & phân tích dữ liệu

**Chức năng**:

- Sales analytics
- Inventory reports
- Dashboard KPIs

**API Endpoints**:

- TBD (Future implementation)

**Dependencies**: Core, All Modules

📄 **Chi tiết**: Xem `Modules/Reporting/README.md`

---

## 🚀 Quick Start

### 1. Build & Run

```bash
# Build project
dotnet build

# Run project
dotnet run
```

### 2. Access APIs

- Swagger UI: `https://localhost:7012/swagger`
- API Base: `https://localhost:7012/api`

### 3. Test Login

```http
POST https://localhost:7012/api/accounts/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin@123"
}
```

## 📚 Design Patterns

### Repository Pattern

- Centralize data access logic
- Abstract database operations
- Easy to mock for testing

### Unit of Work Pattern

- Manage transactions
- Coordinate multiple repositories
- Ensure data consistency

### DTO Pattern

- Data Transfer Objects
- Avoid circular references
- Control exposed data

### Dependency Injection

- Loose coupling
- Easy to test with mocks
- Flexible implementation swap

## 📄 License

MIT License
