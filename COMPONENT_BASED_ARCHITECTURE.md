# Component-Based Architecture - Hệ thống ERP

## Bài tập môn Thiết kế Hệ thống

---

## 1. GIỚI THIỆU

### 1.1. Component-Based Architecture là gì?

**Component-Based Architecture (CBA)** là một phương pháp thiết kế phần mềm trong đó hệ thống được chia nhỏ thành các **components độc lập**, mỗi component đảm nhiệm một tập hợp chức năng cụ thể và có thể tái sử dụng.

### 1.2. Đặc điểm chính

- ✅ **Encapsulation**: Mỗi component che giấu implementation details
- ✅ **Loose Coupling**: Components ít phụ thuộc lẫn nhau
- ✅ **High Cohesion**: Logic liên quan được nhóm lại với nhau
- ✅ **Reusability**: Components có thể tái sử dụng
- ✅ **Replaceable**: Có thể thay thế component mà không ảnh hưởng hệ thống

---

## 2. THIẾT KẾ HỆ THỐNG ERP

### 2.1. Tổng quan kiến trúc

```
┌─────────────────────────────────────────────────────────────┐
│                     ERP SYSTEM                              │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐  │
│  │Identity  │  │Inventory │  │Procurement│  │  Sales   │  │
│  │ Module   │  │  Module  │  │  Module   │  │  Module  │  │
│  └────┬─────┘  └────┬─────┘  └────┬─────┘  └────┬─────┘  │
│       │             │               │             │         │
│       └─────────────┴───────────────┴─────────────┘         │
│                         │                                   │
│                    ┌────▼────┐                             │
│                    │  CORE   │                             │
│                    │Component│                             │
│                    └─────────┘                             │
│                         │                                   │
│                    ┌────▼────┐                             │
│                    │Database │                             │
│                    └─────────┘                             │
└─────────────────────────────────────────────────────────────┘
```

### 2.2. Phân tích Business Components

#### 🔐 **Identity Component**

- **Trách nhiệm**: Authentication & Authorization
- **Chức năng**:
  - User login/logout
  - Role management
  - Permission control
  - Cookie-based session management
- **Public APIs**: `/api/accounts`, `/api/roles`

#### 📦 **Inventory Component**

- **Trách nhiệm**: Quản lý kho hàng & sản phẩm
- **Chức năng**:
  - Product CRUD
  - Category management
  - Warehouse management
  - Stock tracking
  - Inventory reports
- **Public APIs**: `/api/products`, `/api/categories`, `/api/warehouses`

#### 🛒 **Procurement Component**

- **Trách nhiệm**: Quản lý mua hàng
- **Chức năng**:
  - Purchase Order CRUD
  - Supplier management
  - PO approval workflow
  - Purchase analytics
- **Public APIs**: `/api/purchase-orders`, `/api/suppliers`
- **Dependencies**: Inventory (để lấy product info)

#### 💰 **Sales Component**

- **Trách nhiệm**: Quản lý bán hàng
- **Chức năng**:
  - Sales Order CRUD
  - Customer management
  - Order fulfillment
  - Sales analytics
- **Public APIs**: `/api/sales-orders`, `/api/customers`
- **Dependencies**: Inventory (để check stock availability)

#### 👥 **HumanResources Component**

- **Trách nhiệm**: Quản lý nhân sự
- **Chức năng**:
  - Employee CRUD
  - Department management
  - Payroll (future)
  - Attendance (future)
- **Public APIs**: `/api/employees`, `/api/departments`
- **Dependencies**: Identity (để link user account)

#### 📊 **Reporting Component**

- **Trách nhiệm**: Báo cáo & phân tích
- **Chức năng**:
  - Dashboard KPIs
  - Sales reports
  - Inventory reports
  - Financial reports
- **Public APIs**: `/api/reports`
- **Dependencies**: Tất cả modules (để aggregate data)

### 2.3. Core Component

**Core** là shared component chứa các thành phần dùng chung:

```
Core/
├── Common/           # Shared models, enums, utilities
│   ├── Models/       # ResponseData, PageList, BaseSearch
│   ├── Enums/        # ErrorCodeAPI, PermissionEnum
│   └── Utilities/    # Extension methods, helpers
│
├── Database/         # Database infrastructure
│   ├── Entities/     # Domain entities
│   ├── DbContext     # EF Core context
│   ├── BaseRepository # Generic repository
│   └── UnitOfWork    # Unit of Work pattern
│
└── Infrastructure/   # Cross-cutting concerns
    ├── Authorization # Permission handling
    ├── Logging       # Audit logs
    └── Middleware    # Custom middleware
```

---

## 3. NGUYÊN TẮC THIẾT KẾ

### 3.1. Dependency Rule

```
         ┌─────────────┐
         │  Modules    │
         │ (Identity,  │
         │ Inventory,  │
         │   Sales)    │
         └──────┬──────┘
                │ depends on
                ▼
         ┌─────────────┐
         │    CORE     │
         │  Component  │
         └──────┬──────┘
                │ depends on
                ▼
         ┌─────────────┐
         │  Database   │
         └─────────────┘
```

**Quy tắc**:

- ✅ Modules CÓ THỂ phụ thuộc vào Core
- ❌ Core KHÔNG THỂ phụ thuộc vào Modules
- ⚠️ Module-to-Module dependencies nên TỐI THIỂU

### 3.2. Layered Architecture trong mỗi Module

```
┌─────────────────────────────────┐
│      Controllers (API)          │  ← Presentation Layer
├─────────────────────────────────┤
│      Services (Business)        │  ← Business Logic Layer
├─────────────────────────────────┤
│   Repositories (Data Access)    │  ← Data Access Layer
├─────────────────────────────────┤
│   Core.Database (DbContext)     │  ← Database Layer
└─────────────────────────────────┘
```

### 3.3. Communication Pattern

#### Internal Communication (trong cùng module)

```
Controller → Service → Repository → Database
```

#### Cross-Module Communication (giữa các modules)

```
Option 1: Direct Service Call (hiện tại)
Sales.Service → Inventory.Service

Option 2: Event-Driven (future)
Sales.Service → Event Bus → Inventory.Service
```

---

## 4. DESIGN PATTERNS ÁP DỤNG

### 4.1. Repository Pattern

```csharp
// Generic Base Repository
public class BaseRepository<T> where T : class
{
    protected readonly ErpDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public virtual async Task<T> GetByIdAsync(object id)
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    public virtual async Task AddAsync(T entity)
    public virtual async Task UpdateAsync(T entity)
    public virtual async Task DeleteAsync(object id)
}

// Specific Repository
public class ProductRepository : BaseRepository<Product>
{
    // Custom queries cho Product
}
```

**Lợi ích**:

- ✅ Centralize data access logic
- ✅ Dễ dàng mock trong unit tests
- ✅ Giảm duplicate code

### 4.2. Unit of Work Pattern

```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly ErpDbContext _context;

    public IProductRepository Products { get; }
    public ICategoryRepository Categories { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
```

**Lợi ích**:

- ✅ Đảm bảo transaction consistency
- ✅ Giảm số lượng calls đến database

### 4.3. DTO Pattern

```csharp
// Entity (Domain Model)
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Category Category { get; set; }  // Navigation
}

// DTO (Data Transfer Object)
public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; }  // Flattened
}
```

**Lợi ích**:

- ✅ Tránh circular reference khi serialize JSON
- ✅ Kiểm soát data expose ra client
- ✅ Giảm payload size

### 4.4. Dependency Injection

```csharp
// Service Registration
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Constructor Injection
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepo;

    public ProductService(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }
}
```

**Lợi ích**:

- ✅ Loose coupling
- ✅ Dễ test với mock objects
- ✅ Dễ thay đổi implementation

---

## 5. SO SÁNH VỚI CÁC KIẾN TRÚC KHÁC

### 5.1. Component-Based vs Monolithic

| Aspect          | Monolithic       | Component-Based                 |
| --------------- | ---------------- | ------------------------------- |
| **Structure**   | Single codebase  | Organized modules               |
| **Deployment**  | Deploy all       | Deploy all (có thể split later) |
| **Development** | Simple initially | Better long-term                |
| **Scalability** | Scale entire app | Scale by component (future)     |
| **Maintenance** | Harder as grows  | Easier with clear boundaries    |

### 5.2. Component-Based vs Microservices

| Aspect             | Component-Based                | Microservices           |
| ------------------ | ------------------------------ | ----------------------- |
| **Deployment**     | Single application             | Separate services       |
| **Database**       | Shared database                | Database per service    |
| **Communication**  | In-process calls               | HTTP/gRPC/Message Queue |
| **Complexity**     | Lower                          | Higher                  |
| **Team Size**      | Small to Medium                | Medium to Large         |
| **Migration Path** | ✅ Can evolve to microservices | Final state             |

### 5.3. Component-Based vs Layered

| Aspect             | Layered                                | Component-Based               |
| ------------------ | -------------------------------------- | ----------------------------- |
| **Organization**   | By technical layer                     | By business domain            |
| **Example**        | Controllers/, Services/, Repositories/ | Identity/, Sales/, Inventory/ |
| **Coupling**       | Cross-layer                            | Within component              |
| **Business Logic** | Scattered                              | Centralized per domain        |

---

## 6. LỢI ÍCH CỦA COMPONENT-BASED ARCHITECTURE

### 6.1. Maintainability (Dễ bảo trì)

✅ **Code Organization**: Code được nhóm theo business domain

```
Muốn fix bug về Purchase Order?
→ Vào Procurement module
→ Tất cả code liên quan đều ở đây!
```

✅ **Clear Boundaries**: Ranh giới rõ ràng giữa các components

```
Identity code KHÔNG bao giờ bị lẫn vào Sales code
```

### 6.2. Scalability (Khả năng mở rộng)

✅ **Horizontal Scaling**: Scale từng component riêng

```
Inventory module bị quá tải?
→ Có thể tách thành separate service sau này
```

✅ **Feature Addition**: Thêm module mới không ảnh hưởng modules cũ

```
Cần thêm Accounting module?
→ Tạo folder mới Modules/Accounting
→ Không cần sửa code Identity, Sales, etc.
```

### 6.3. Reusability (Tái sử dụng)

✅ **Shared Components**: Core được dùng bởi tất cả modules

```
ResponseData, PageList, BaseRepository
→ Write once, use everywhere
```

✅ **Cross-Module Usage**: Modules có thể dùng services của nhau

```
Sales cần check stock?
→ Gọi Inventory.ProductService
```

### 6.4. Testability (Dễ test)

✅ **Unit Testing**: Test từng component độc lập

```csharp
// Test ProductService không cần care về SalesService
[Test]
public async Task CreateProduct_ShouldReturnSuccess()
{
    var mockRepo = new Mock<IProductRepository>();
    var service = new ProductService(mockRepo.Object);

    var result = await service.CreateAsync(productDto);

    Assert.That(result.Success, Is.True);
}
```

✅ **Integration Testing**: Test theo business flow

```csharp
// Test flow: Create SO → Check inventory → Update stock
[Test]
public async Task CreateSalesOrder_ShouldDecreaseStock()
{
    // Arrange: Setup test data
    // Act: Create sales order
    // Assert: Verify stock decreased
}
```

### 6.5. Team Collaboration

✅ **Parallel Development**: Teams làm việc độc lập

```
Team A: Identity module
Team B: Inventory module
Team C: Sales module
→ Không conflict với nhau!
```

✅ **Code Ownership**: Mỗi team chịu trách nhiệm cho component của mình

```
Bug trong Purchase Order?
→ Procurement team sẽ fix
→ Không cần involve Sales team
```

---

## 7. DEMO IMPLEMENTATION

### 7.1. Ví dụ: Sales Module

#### Structure

```
Modules/Sales/
├── Controllers/
│   ├── SalesOrdersController.cs
│   └── CustomerController.cs
├── Services/
│   ├── SalesOrderService.cs
│   └── CustomerService.cs
├── Repositories/
│   ├── SalesOrderRepository.cs
│   └── CustomerRepository.cs
├── DTOs/
│   └── SalesOrder/
│       ├── CreateSalesOrderDto.cs
│       ├── UpdateSalesOrderDto.cs
│       └── SalesOrderResponseDto.cs
└── Mappers/
    └── SalesOrderMapper.cs
```

#### Data Flow Example

```csharp
// 1. Controller nhận request
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateSalesOrderDto dto)
{
    var result = await _salesOrderService.CreateAsync(dto);
    return Ok(result);
}

// 2. Service xử lý business logic
public async Task<ResponseData<SalesOrderResponseDto>> CreateAsync(CreateSalesOrderDto dto)
{
    // Validate
    if (dto.Details.Count == 0)
        return new ResponseData { Success = false, Message = "Need items" };

    // Check stock availability (call Inventory module)
    var hasStock = await _inventoryService.CheckStockAsync(dto.Details);
    if (!hasStock)
        return new ResponseData { Success = false, Message = "Out of stock" };

    // Create order
    var order = _mapper.ToEntity(dto);
    await _repository.AddAsync(order);

    // Decrease inventory (call Inventory module)
    await _inventoryService.DecreaseStockAsync(dto.Details);

    return new ResponseData<SalesOrderResponseDto>
    {
        Success = true,
        Data = _mapper.ToDto(order)
    };
}

// 3. Repository save to database
public async Task AddAsync(SalesOrder entity)
{
    await _dbSet.AddAsync(entity);
    await _context.SaveChangesAsync();
}
```

---

## 8. HƯỚNG PHÁT TRIỂN

### 8.1. Phase 1: Hiện tại - Component-Based Monolith

```
┌─────────────────────────────┐
│   Single Application        │
│  ┌────┐ ┌────┐ ┌────┐      │
│  │Mod1│ │Mod2│ │Mod3│      │
│  └────┘ └────┘ └────┘      │
│         ┌──────┐            │
│         │  DB  │            │
│         └──────┘            │
└─────────────────────────────┘
```

### 8.2. Phase 2: Service-Oriented Architecture

```
┌─────────────┐
│  API Gateway│
└──────┬──────┘
       │
   ┌───┴───┬───────┬───────┐
   │       │       │       │
┌──▼──┐ ┌─▼───┐ ┌─▼───┐ ┌─▼───┐
│Mod1 │ │Mod2 │ │Mod3 │ │Mod4 │
└──┬──┘ └──┬──┘ └──┬──┘ └──┬──┘
   │       │       │       │
   └───────┴───┬───┴───────┘
           ┌───▼───┐
           │   DB  │
           └───────┘
```

### 8.3. Phase 3: Microservices

```
┌─────────────┐
│  API Gateway│
└──────┬──────┘
       │
   ┌───┴───┬───────┬───────┐
   │       │       │       │
┌──▼──┐ ┌─▼───┐ ┌─▼───┐ ┌─▼───┐
│Svc1 │ │Svc2 │ │Svc3 │ │Svc4 │
└──┬──┘ └──┬──┘ └──┬──┘ └──┬──┘
┌──▼──┐ ┌─▼───┐ ┌─▼───┐ ┌─▼───┐
│ DB1 │ │ DB2 │ │ DB3 │ │ DB4 │
└─────┘ └─────┘ └─────┘ └─────┘
```

---

## 9. KẾT LUẬN

### 9.1. Tại sao chọn Component-Based cho ERP?

✅ **Business Domain Complexity**: ERP có nhiều business domains (Sales, Inventory, HR, etc.)
✅ **Team Scalability**: Dễ dàng scale team khi business lớn
✅ **Long-term Maintainability**: Dễ maintain khi codebase lớn
✅ **Migration Path**: Có thể evolve sang microservices sau này

### 9.2. Lessons Learned

1. **Start Simple**: Bắt đầu với clear module boundaries
2. **Minimize Dependencies**: Giảm thiểu phụ thuộc giữa modules
3. **Shared Core**: Đầu tư vào Core component chất lượng cao
4. **Document Well**: Document rõ ràng responsibilities của từng component
5. **Refactor Continuously**: Liên tục refactor để improve architecture

### 9.3. Key Takeaways

> **Component-Based Architecture** là sweet spot giữa Monolithic và Microservices:
>
> - Đủ đơn giản để implement
> - Đủ flexible để scale
> - Dễ dàng evolve khi business grows

---

## 📚 TÀI LIỆU THAM KHẢO

1. **Books**:

   - "Clean Architecture" by Robert C. Martin
   - "Domain-Driven Design" by Eric Evans
   - "Building Microservices" by Sam Newman

2. **Articles**:

   - [Component-Based Software Engineering](https://en.wikipedia.org/wiki/Component-based_software_engineering)
   - [Modular Monoliths](https://www.kamilgrzybek.com/design/modular-monolith-primer/)
   - [Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/)

3. **Videos**:
   - [Clean Architecture - Uncle Bob](https://www.youtube.com/watch?v=Nsjsiz2A9mg)
   - [Modular Monoliths](https://www.youtube.com/watch?v=5OjqD-ow8GE)

---

**Được tạo cho bài tập môn Thiết kế Hệ thống**
_Component-Based Architecture Implementation - ERP System_
