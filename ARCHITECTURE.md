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

## 🔮 Future Enhancements

### Phase 1: Component Refinement

- [ ] Implement DTOs cho tất cả modules
- [ ] Add validation attributes
- [ ] Implement AutoMapper
- [ ] Add unit tests

### Phase 2: Advanced Patterns

- [ ] CQRS (Command Query Responsibility Segregation)
- [ ] MediatR for inter-module communication
- [ ] Event-driven architecture
- [ ] Domain events

### Phase 3: Microservices Migration

- [ ] Extract modules thành separate APIs
- [ ] Implement API Gateway
- [ ] Service discovery
- [ ] Distributed transactions (Saga pattern)

## 📚 Tài liệu tham khảo

- [Component-Based Architecture](https://en.wikipedia.org/wiki/Component-based_software_engineering)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [Microservices Pattern](https://microservices.io/patterns/index.html)

## 👥 Contributors

- tdevip666

## 📄 License

MIT License
