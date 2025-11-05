# Quick Start Guide - Component-Based ERP System

## 📋 Tổng quan nhanh

Project ERP-API đã được refactor thành **Component-Based Architecture** với 7 business modules:

- 🔐 **Identity** - Authentication & Authorization
- 📦 **Inventory** - Products & Warehouses
- 🛒 **Procurement** - Purchase Orders
- 💰 **Sales** - Sales Orders
- 👥 **HumanResources** - Employees & Departments
- 📊 **Reporting** - Analytics & Reports
- ⚙️ **Core** - Shared Components

---

## 🚀 Bắt đầu nhanh

### 1. Build Project

```powershell
dotnet build
```

✅ **Expected**: Build successful with 0 errors (có thể có warnings về nullable)

### 2. Run Project

```powershell
dotnet run
```

🌐 **Access**:

- Swagger UI: `https://localhost:7012/swagger`
- API Base: `https://localhost:7012/api`

### 3. Test API

#### Login

```http
POST /api/accounts/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin@123"
}
```

**Note**: Login sẽ tự động set cookie, không cần thêm Authorization header.

#### Get Products (với cookie authentication)

```http
GET /api/products
Cookie: .AspNetCore.Identity.Application={cookie-value}
```

**Note**: Browser/Postman sẽ tự động gửi cookie sau khi login thành công.

---

## 📂 Cấu trúc Project

```
ERP-API/
├── Core/                    # Shared components
│   ├── Common/              # Models, Enums, Utilities
│   ├── Database/            # Entities, DbContext, Repositories
│   └── Infrastructure/      # Authorization, Middleware
│
├── Modules/                 # Business components
│   ├── Identity/            # Auth module
│   ├── Inventory/           # Product & Warehouse module
│   ├── Procurement/         # Purchase module
│   ├── Sales/               # Sales module
│   ├── HumanResources/      # HR module
│   └── Reporting/           # Reporting module
│
└── Documentation/
    ├── ARCHITECTURE.md                   # Architecture overview
    ├── COMPONENT_BASED_ARCHITECTURE.md   # Detailed CBA doc
    ├── DIAGRAMS.md                       # Visual diagrams
    ├── PRESENTATION_SLIDES.md            # Presentation materials
    └── REFACTORING_SUMMARY.md            # Summary of changes
```

---

## 🎯 Các Module chính

### Identity Module

**Location**: `Modules/Identity/`

**Controllers**:

- `AccountsController` - Login, Register, User management
- `RoleController` - Role & Permission management

**Endpoints**:

- `POST /api/accounts/login` - Login
- `POST /api/accounts/register` - Register
- `GET /api/roles` - Get all roles

---

### Inventory Module

**Location**: `Modules/Inventory/`

**Controllers**:

- `ProductsController` - Product CRUD
- `CategoryController` - Category management
- `WarehouseController` - Warehouse management
- `WarehouseReportController` - Warehouse reports

**Endpoints**:

- `GET /api/products` - List products
- `POST /api/products` - Create product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

---

### Procurement Module

**Location**: `Modules/Procurement/`

**Controllers**:

- `PurchaseOrdersController` - Purchase order CRUD
- `SupplierController` - Supplier management

**Endpoints**:

- `GET /api/purchase-orders` - List purchase orders
- `POST /api/purchase-orders` - Create PO
- `GET /api/suppliers` - List suppliers

---

### Sales Module

**Location**: `Modules/Sales/`

**Controllers**:

- `SalesOrdersController` - Sales order CRUD
- `CustomerController` - Customer management

**Endpoints**:

- `GET /api/sales-orders` - List sales orders
- `POST /api/sales-orders` - Create SO
- `GET /api/customers` - List customers

---

### HumanResources Module

**Location**: `Modules/HumanResources/`

**Controllers**:

- `EmployeeController` - Employee CRUD
- `DepartmentController` - Department management

**Endpoints**:

- `GET /api/employees` - List employees
- `POST /api/employees` - Create employee
- `GET /api/departments` - List departments

---

## 🔍 Tìm code nhanh

### Muốn thêm API endpoint mới?

1. **Xác định module** - Endpoint thuộc module nào?
2. **Thêm trong Controller** - `Modules/{ModuleName}/Controllers/`
3. **Implement trong Service** - `Modules/{ModuleName}/Services/`
4. **Data access trong Repository** - `Modules/{ModuleName}/Repositories/`

### Ví dụ: Thêm "Get Product by Category"

```
📁 Modules/Inventory/
   ├── Controllers/
   │   └── ProductsController.cs
   │       → Add [HttpGet("category/{categoryId}")]
   │
   ├── Services/
   │   └── ProductService.cs
   │       → Add GetByCategoryAsync(int categoryId)
   │
   └── Repositories/
       └── [Use existing ProductRepository]
```

---

## 🛠️ Development Workflow

### 1. Tạo feature mới

```bash
# 1. Create new branch
git checkout -b feature/your-feature-name

# 2. Identify module
# Example: Adding new report → Reporting module

# 3. Add code in module
#    - Controller for API endpoint
#    - Service for business logic
#    - Repository for data access (if needed)
#    - DTOs for request/response

# 4. Test
dotnet build
dotnet run

# 5. Commit
git add .
git commit -m "feat: add your feature"
```

### 2. Thêm entity mới

```bash
# 1. Add entity class
📁 Core/Database/Entities/YourEntity.cs

# 2. Add to DbContext
📁 Core/Database/Entities/ErpDbContext.cs
# → Add DbSet<YourEntity>

# 3. Create migration
dotnet ef migrations add AddYourEntity

# 4. Update database
dotnet ef database update
```

### 3. Debug

```bash
# VS Code: Press F5
# Or Terminal:
dotnet run --launch-profile "https"

# View logs in console
# Test in Swagger: https://localhost:7012/swagger
```

---

## 📚 Tài liệu chi tiết

### Cho development

- `ARCHITECTURE.md` - Hiểu tổng quan architecture
- Module READMEs - Hiểu chi tiết từng module
  - `Modules/Identity/README.md`
  - `Modules/Inventory/README.md`
  - `Modules/Procurement/README.md`
  - `Modules/Sales/README.md`
  - `Modules/HumanResources/README.md`
  - `Modules/Reporting/README.md`

### Cho bài tập môn học

- `COMPONENT_BASED_ARCHITECTURE.md` - Lý thuyết và giải thích chi tiết
- `DIAGRAMS.md` - Diagrams cho báo cáo
- `PRESENTATION_SLIDES.md` - Slides cho thuyết trình
- `REFACTORING_SUMMARY.md` - Tóm tắt quá trình refactor

---

## 🐛 Troubleshooting

### Build errors

```bash
# Clear build cache
dotnet clean
dotnet build
```

### Database connection error

1. Check `appsettings.json` → ConnectionStrings
2. Ensure MySQL server is running
3. Run migrations: `dotnet ef database update`

### Cannot access API

1. Check if app is running: `dotnet run`
2. Check port: Should be `7012` (HTTPS) or `5274` (HTTP)
3. Check firewall settings

### Cookie Authentication issues

1. Ensure you called `/api/accounts/login` first
2. Check that login was successful (200 OK)
3. In browser: Cookies are sent automatically
4. In Postman/Tools: Enable "Save cookies" option
5. Check cookie `.AspNetCore.Identity.Application` exists

---

## 📊 Testing

### Unit Tests (Future)

```bash
# Create test project
dotnet new xunit -n ERP-API.Tests

# Add test for a module
# Example: Tests/Identity/AccountServiceTests.cs

dotnet test
```

### Integration Tests (Future)

```bash
# Test full flow
# Example: Create Sales Order → Check Stock → Update Inventory

dotnet test --filter Category=Integration
```

---

## 🎓 Learning Path

### Beginner

1. ✅ Hiểu cấu trúc folder
2. ✅ Đọc `ARCHITECTURE.md`
3. ✅ Chạy và test API trong Swagger
4. ✅ Xem code của 1 module (bắt đầu với Identity)

### Intermediate

1. ✅ Hiểu data flow (request → controller → service → repository)
2. ✅ Thêm API endpoint mới
3. ✅ Hiểu cross-module communication
4. ✅ Đọc `COMPONENT_BASED_ARCHITECTURE.md`

### Advanced

1. ✅ Refactor existing code
2. ✅ Implement CQRS pattern
3. ✅ Add event-driven communication
4. ✅ Prepare for microservices migration

---

## 🔗 Useful Links

### Documentation

- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

### Tools

- [Swagger/OpenAPI](https://swagger.io/)
- [Postman](https://www.postman.com/) - API testing
- [DBeaver](https://dbeaver.io/) - Database management

---

## 💡 Tips

### Performance

- ✅ Use async/await cho I/O operations
- ✅ Implement caching cho frequently accessed data
- ✅ Use pagination cho large datasets
- ✅ Add database indexes cho search fields

### Security

- ✅ Always validate input
- ✅ Use parameterized queries (EF Core does this)
- ✅ Implement rate limiting
- ✅ Use HTTPS in production
- ✅ Cookie-based auth is secure (see `COOKIE_AUTHENTICATION.md`)

### Code Quality

- ✅ Follow naming conventions
- ✅ Write meaningful comments
- ✅ Keep methods small (< 50 lines)
- ✅ Use dependency injection
- ✅ Handle errors properly

---

## ✅ Checklist khi bắt đầu

- [ ] Clone/Pull latest code
- [ ] Restore packages: `dotnet restore`
- [ ] Build project: `dotnet build`
- [ ] Update database: `dotnet ef database update`
- [ ] Run project: `dotnet run`
- [ ] Open Swagger: `https://localhost:7012/swagger`
- [ ] Test login API
- [ ] Read `ARCHITECTURE.md`
- [ ] Explore one module code

---

## 🎉 Ready to Code!

Bây giờ bạn đã sẵn sàng để:

- ✅ Develop features mới
- ✅ Fix bugs
- ✅ Refactor code
- ✅ Write tests
- ✅ Deploy to production

**Happy Coding!** 🚀

---

**Last Updated**: November 5, 2025  
**Version**: 1.0.0  
**Build Status**: ✅ Passing
