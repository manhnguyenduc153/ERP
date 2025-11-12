# ERP API - Design Patterns Analysis

## Tổng quan

Project ERP API hiện đang sử dụng **11 Design Patterns** chính được phân bố trong các layer khác nhau của ứng dụng, tuân theo **Clean Architecture** và **SOLID Principles**.

---

## 📋 Danh sách Design Patterns

### 1. **Repository Pattern**

**Vị trí**: `Core/Database/BaseRepository.cs`, `Modules/*/Repositories/`
**Mục đích**: Tách biệt logic truy cập dữ liệu khỏi business logic

```csharp
// Base Repository - Generic implementation
public class BaseRepository<T, TContext> where T : class where TContext : DbContext
{
    protected readonly TContext _dbContext;
    protected readonly IUnitOfWork _unitOfWork;

    public async Task<T?> GetByIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();
}

// Concrete Repository
public class CategoryRepository : BaseRepository<Category, ErpDbContext>, ICategoryRepository
{
    public CategoryRepository(ErpDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork) { }
}
```

**Modules sử dụng**:

- `Modules/Identity/Repositories/` - Account management
- `Modules/Sales/Repositories/` - Customer, SalesOrder, SaleStaff
- `Modules/Procurement/Repositories/` - Supplier, PurchaseOrder, PurchaseStaff
- `Modules/Inventory/Repositories/` - Product, Category, Warehouse, Stock
- `Modules/HumanResources/Repositories/` - Employee, Department

---

### 2. **Unit of Work Pattern**

**Vị trí**: `Core/Database/UnitOfWork.cs`, `Core/Database/IUnitOfWork.cs`
**Mục đích**: Quản lý transactions và đảm bảo data consistency

```csharp
public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _dbContext;
    private IDbContextTransaction? _transaction;

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_transaction == null)
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        return _transaction;
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _dbContext.SaveChangesAsync();
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
```

**Lợi ích**:

- Đảm bảo ACID properties
- Quản lý transactions tập trung
- Rollback dễ dàng khi có lỗi

---

### 3. **Generic Pattern**

**Vị trí**: `Core/Database/BaseRepository.cs`, `Core/Common/Models/ResponseData.cs`
**Mục đích**: Tái sử dụng code cho nhiều entity types

```csharp
// Generic Repository
public class BaseRepository<T, TContext> where T : class where TContext : DbContext

// Generic Response wrapper
public class ResponseData<T> where T : class
{
    public T? Data { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
}
```

**Tính năng**:

- Type safety
- Code reusability
- Consistent API responses

---

### 4. **Dependency Injection Pattern**

**Vị trí**: `Extensions/ServicesRegister.cs`, Controllers, Services
**Mục đích**: Loose coupling và IoC (Inversion of Control)

```csharp
public static class ServicesRegister
{
    public static void RegisterCustomServices(this IServiceCollection services)
    {
        // Core Infrastructure
        services.AddScoped<IUnitOfWork, UnitOfWork<ErpDbContext>>();

        // Repositories
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();

        // Services
        services.AddScoped<AccountService>();
        services.AddScoped<CategoryService>();
    }
}
```

**Service Lifetimes**:

- **Transient**: Repositories (mới tạo mỗi lần inject)
- **Scoped**: Services, UnitOfWork (per request)
- **Singleton**: Authorization handlers

---

### 5. **Factory Pattern (ASP.NET Identity)**

**Vị trí**: `Program.cs` - ASP.NET Identity configuration
**Mục đích**: Tạo và cấu hình User/Role objects

```csharp
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ErpDbContext>()
    .AddDefaultTokenProviders();
```

**Chức năng**:

- Tạo User managers
- Password validation
- Token generation

---

### 6. **Template Method Pattern**

**Vị trí**: `Core/Database/BaseRepository.cs`
**Mục đích**: Định nghĩa skeleton algorithm, cho phép subclass override

```csharp
// Base template
public class BaseRepository<T, TContext>
{
    // Template methods
    public virtual async Task<T?> GetByIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id);
    public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();
}

// Specific implementation can override
public class CategoryRepository : BaseRepository<Category, ErpDbContext>
{
    // Can override base methods or add specific ones
    public async Task<List<Category>> GetCategoriesAsync() => await _dbContext.Categories.ToListAsync();
}
```

---

### 7. **Strategy Pattern (Data Access)**

**Vị trí**: `Core/Database/BaseRepository.cs`
**Mục đích**: Multiple data access strategies

```csharp
public class BaseRepository<T, TContext>
{
    // EF Core Strategy
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbContext.Set<T>().ToListAsync();

    // Dapper Strategy
    public async Task<IEnumerable<TResult>> DapperQueryAsync<TResult>(string sql, object? param = null) =>
        await Connection.QueryAsync<TResult>(sql, param);

    // Query Strategy with/without tracking
    public IQueryable<T> FindAll(bool trackChanges = false) =>
        trackChanges ? _dbContext.Set<T>() : _dbContext.Set<T>().AsNoTracking();
}
```

**Strategies**:

- **EF Core**: Cho CRUD operations
- **Dapper**: Cho complex queries, performance-critical operations
- **NoTracking**: Cho read-only queries

---

### 8. **Decorator Pattern (Authorization)**

**Vị trí**: `Core/Infrastructure/HasPermissionAttribute.cs`
**Mục đích**: Thêm authorization logic vào controllers/actions

```csharp
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission)
    {
        Policy = $"Permission.{permission}";
    }
}

// Usage
[HasPermission(Permission.ViewUsers)]
[HttpGet]
public async Task<IActionResult> GetUsers() { }
```

**Tính năng**:

- Declarative security
- Reusable authorization logic
- Clean separation of concerns

---

### 9. **Handler Pattern (Authorization)**

**Vị trí**: `Core/Infrastructure/PermissionAuthorization.cs`
**Mục đích**: Xử lý authorization requirements

```csharp
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Custom authorization logic
        var user = await _userManager.GetUserAsync(context.User);
        var userRoles = await _userManager.GetRolesAsync(user);

        // Check permissions in roles
        foreach (var roleName in userRoles)
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            if (claims.Any(c => c.Type == "Permission" && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
```

---

### 10. **Extension Method Pattern**

**Vị trí**: `Extensions/ServicesRegister.cs`, `Core/Common/Utilities/`
**Mục đích**: Mở rộng functionality cho existing types

```csharp
// Service registration extension
public static class ServicesRegister
{
    public static void RegisterCustomServices(this IServiceCollection services) { }
}

// Usage
builder.Services.RegisterCustomServices();

// Enum extension
public static class EnumExtensions
{
    public static string GetDescription(this Enum value) { }
}
```

---

### 11. **Module Pattern (Clean Architecture)**

**Vị trí**: `Modules/` folder structure
**Mục đích**: Tổ chức code theo business domains

```
Modules/
├── Identity/           # Authentication & Authorization
├── Sales/             # Sales Orders & Customers
├── Procurement/       # Purchase Orders & Suppliers
├── Inventory/         # Products, Categories, Warehouses
├── HumanResources/    # Employees & Departments
└── Reporting/         # Analytics & Reports
```

**Mỗi module có**:

- Controllers (API layer)
- Services (Business logic)
- Repositories (Data access)
- DTOs (Data transfer objects)

---

## 🏗️ Architecture Patterns

### **Clean Architecture**

**Cấu trúc**:

```
Core/                  # Inner layer - Domain & Infrastructure
├── Common/           # Shared models, enums, utilities
├── Database/         # Data access abstractions
└── Infrastructure/   # Cross-cutting concerns

Modules/              # Outer layer - Application features
├── Identity/
├── Sales/
├── Procurement/
├── Inventory/
└── HumanResources/
```

**Dependency Rule**: Modules → Core (ALLOWED), Core → Modules (FORBIDDEN)

### **Layered Architecture (trong mỗi module)**

```
Controllers (API Layer)
    ↓
Services (Business Logic Layer)
    ↓
Repositories (Data Access Layer)
    ↓
Core.Database (Database Layer)
```

---

## 🎯 SOLID Principles Implementation

### **Single Responsibility Principle (SRP)**

- Mỗi class có 1 nhiệm vụ duy nhất
- Repository chỉ lo data access
- Service chỉ lo business logic
- Controller chỉ lo HTTP handling

### **Open/Closed Principle (OCP)**

- BaseRepository có thể extend mà không modify
- Modules có thể thêm mới mà không ảnh hưởng existing code

### **Liskov Substitution Principle (LSP)**

- Concrete repositories có thể thay thế interfaces
- Generic constraints đảm bảo type safety

### **Interface Segregation Principle (ISP)**

- Interfaces nhỏ và cụ thể (IAccountRepository, ICategoryRepository)
- Không force implement unused methods

### **Dependency Inversion Principle (DIP)**

- High-level modules depend on abstractions (IUnitOfWork, IRepository)
- Low-level modules implement abstractions

---

## 📊 Pattern Usage Statistics

| Pattern              | Số lượng implementations | Tần suất sử dụng |
| -------------------- | ------------------------ | ---------------- |
| Repository           | 15+ repositories         | Rất cao          |
| Dependency Injection | 50+ registrations        | Rất cao          |
| Generic              | 10+ generic classes      | Cao              |
| Unit of Work         | 1 implementation         | Cao              |
| Strategy             | 3 strategies             | Trung bình       |
| Decorator            | 1 attribute              | Trung bình       |
| Handler              | 1 handler                | Trung bình       |
| Template Method      | Base classes             | Trung bình       |
| Extension Method     | 5+ extensions            | Trung bình       |
| Factory              | Identity factories       | Thấp             |
| Module               | 5 modules                | Cao              |

---

## 🚀 Lợi ích của Design Patterns trong Project

### **Maintainability**

- Code dễ đọc, dễ hiểu
- Separation of concerns rõ ràng
- Easy to locate and fix bugs

### **Scalability**

- Dễ thêm modules mới
- Horizontal scaling by modules
- Can migrate to microservices

### **Testability**

- Dependency injection enables mocking
- Each layer can be tested independently
- Clean interfaces for unit testing

### **Reusability**

- Generic base classes
- Shared utilities and models
- Common patterns across modules

### **Team Collaboration**

- Consistent code structure
- Clear boundaries between modules
- Multiple teams can work in parallel

---

## 📝 Best Practices Được Áp Dụng

1. **Repository Pattern**: Centralized data access
2. **Unit of Work**: Transaction management
3. **Dependency Injection**: Loose coupling
4. **Clean Architecture**: Domain-driven design
5. **Generic Programming**: Type safety + reusability
6. **Authorization Patterns**: Security by design
7. **Extension Methods**: Fluent APIs
8. **Module Pattern**: Business domain separation

---

## 🔮 Recommendations for Future

### **Patterns có thể thêm**:

1. **Command Pattern**: Cho complex business operations
2. **Mediator Pattern**: Decouple module communications
3. **Observer Pattern**: Event-driven architecture
4. **Builder Pattern**: Complex object construction
5. **Specification Pattern**: Business rules encapsulation

### **Architecture improvements**:

1. **CQRS**: Separate read/write models
2. **Event Sourcing**: Audit trails
3. **Saga Pattern**: Distributed transactions
4. **Circuit Breaker**: Resilience patterns

---

_Generated on: ${new Date().toISOString()}_
_Project: ERP API_
_Architecture: Clean Architecture with Component-Based Design_
