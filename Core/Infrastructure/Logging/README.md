# Audit Logging with Decorator Pattern

## 📋 Overview

Audit Logging system được thiết kế theo **Decorator Pattern** để cho phép thêm các tính năng logging một cách linh hoạt mà không cần thay đổi core implementation.

## 🎯 Decorator Pattern Implementation

### Cấu trúc

```
Core/Infrastructure/Logging/
├── IAuditLogger.cs                     # Core interface
├── AuditLogEntry.cs                    # Log entry model
├── DatabaseAuditLogger.cs              # Base implementation
├── AuditLoggerDecorator.cs             # Abstract decorator
└── Decorators/
    ├── ConsoleAuditLoggerDecorator.cs  # Console output
    ├── FileAuditLoggerDecorator.cs     # File logging
    └── PerformanceAuditLoggerDecorator.cs # Performance monitoring
```

### Component Diagram

```
┌─────────────────────────────────────────────────────────┐
│                   IAuditLogger                          │
│  + LogAsync(AuditLogEntry)                             │
│  + GetListPagingAsync(search)                          │
│  + GetByIdAsync(id)                                     │
│  + ExportToCsvAsync(search)                            │
└─────────────────────────────────────────────────────────┘
                        ▲
                        │
        ┌───────────────┼───────────────┐
        │                               │
┌───────────────────┐         ┌─────────────────────┐
│ DatabaseAuditLogger│         │ AuditLoggerDecorator│
│ (Base Component)   │         │  (Abstract Decorator)│
└───────────────────┘         └─────────────────────┘
                                        ▲
                                        │
                    ┌───────────────────┼───────────────────┐
                    │                   │                   │
        ┌───────────────────┐ ┌─────────────────┐ ┌──────────────────┐
        │ ConsoleDecorator  │ │ FileDecorator   │ │ PerformanceDecorator│
        └───────────────────┘ └─────────────────┘ └──────────────────┘
```

## 🔧 Usage

### 1. Basic Setup (in ServicesRegister.cs)

```csharp
services.AddScoped<IAuditLogger>(provider =>
{
    var dbContext = provider.GetRequiredService<ErpDbContext>();
    
    // Core logger
    IAuditLogger logger = new DatabaseAuditLogger(dbContext);
    
    // Add decorators (order matters!)
    logger = new PerformanceAuditLoggerDecorator(logger, warningThresholdMs: 500);
    logger = new FileAuditLoggerDecorator(logger);
    logger = new ConsoleAuditLoggerDecorator(logger, enableColors: true);
    
    return logger;
});
```

### 2. Using in Service

```csharp
public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogger _auditLogger;
    
    public AuditLogService(IAuditLogger auditLogger)
    {
        _auditLogger = auditLogger;
    }
    
    public async Task LogAsync(string action, string endpoint, 
        object? oldValue = null, object? newValue = null)
    {
        var logEntry = new AuditLogEntry
        {
            Action = action,
            Endpoint = endpoint,
            UserId = userId,
            OldValue = oldValue,
            NewValue = newValue,
            Status = status,
            CreatedAt = DateTime.UtcNow
        };
        
        await _auditLogger.LogAsync(logEntry);
    }
}
```

### 3. Using in Controller

```csharp
[HttpPost]
public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
{
    var result = await _productService.CreateAsync(dto);
    
    // Log the action
    await _auditLogService.LogAsync(
        action: "CREATE_PRODUCT",
        endpoint: "/api/products",
        oldValue: null,
        newValue: result
    );
    
    return Ok(result);
}
```

## 🎨 Available Decorators

### 1. DatabaseAuditLogger (Base Component)

**Purpose**: Lưu audit logs vào database

**Features**:
- Persist logs to MySQL database
- Support querying and filtering
- CSV export functionality

**Configuration**: Automatic via DbContext

### 2. ConsoleAuditLoggerDecorator

**Purpose**: Hiển thị logs ra console với màu sắc

**Features**:
- Colored output (green for SUCCESS, red for FAILED)
- Timestamp formatting
- User and action display

**Configuration**:
```csharp
logger = new ConsoleAuditLoggerDecorator(logger, enableColors: true);
```

**Output Example**:
```
[2025-11-06 10:30:45] ✓ [SUCCESS] User: admin | Action: CREATE_PRODUCT | Endpoint: /api/products
[2025-11-06 10:31:20] ✗ [FAILED] User: user123 | Action: DELETE_ORDER | Endpoint: /api/orders/5
```

### 3. FileAuditLoggerDecorator

**Purpose**: Ghi logs vào file theo ngày

**Features**:
- Daily log rotation (audit_YYYYMMDD.log)
- Thread-safe file writing
- JSON serialization for complex objects

**Configuration**:
```csharp
logger = new FileAuditLoggerDecorator(
    logger,
    logDirectory: "Logs/Audit",  // Optional
    logFilePrefix: "audit"        // Optional
);
```

**Log Location**: `Logs/Audit/audit_20251106.log`

**File Format**:
```
[2025-11-06 10:30:45] [SUCCESS] User: admin | Action: CREATE_PRODUCT | Endpoint: /api/products | Old: null | New: {"id":123,"name":"Product A"}
```

### 4. PerformanceAuditLoggerDecorator

**Purpose**: Đo thời gian thực thi và cảnh báo nếu chậm

**Features**:
- Measure logging execution time
- Warning if exceeds threshold
- Performance metrics

**Configuration**:
```csharp
logger = new PerformanceAuditLoggerDecorator(
    logger,
    warningThresholdMs: 500  // 500ms threshold
);
```

**Output Example**:
```
[PERFORMANCE] Audit logging completed in 125ms
[PERFORMANCE_WARNING] Audit logging took 750ms (threshold: 500ms) for action: CREATE_ORDER
```

## 🔄 Decorator Chain Order

**Execution flow** (outside to inside):

```
Console Decorator
    ↓ (writes to console)
File Decorator
    ↓ (writes to file)
Performance Decorator
    ↓ (measures time)
Database Logger
    ↓ (saves to DB)
```

**Log Entry** → Console → File → Performance → Database

## 📊 Benefits of Decorator Pattern

### 1. **Flexibility**
- Dễ dàng thêm/bỏ decorators
- Có thể thay đổi thứ tự decorators
- Không cần sửa đổi existing code

### 2. **Single Responsibility**
- Mỗi decorator có một nhiệm vụ cụ thể
- DatabaseLogger: Lưu DB
- ConsoleDecorator: Console output
- FileDecorator: File logging
- PerformanceDecorator: Performance monitoring

### 3. **Open/Closed Principle**
- Open for extension (thêm decorators mới)
- Closed for modification (không sửa base logger)

### 4. **Composability**
- Có thể kết hợp nhiều decorators
- Tạo các logging strategies khác nhau

## 🎯 Custom Decorator Example

Tạo decorator mới (ví dụ: EmailAuditLoggerDecorator):

```csharp
public class EmailAuditLoggerDecorator : AuditLoggerDecorator
{
    private readonly IEmailService _emailService;
    
    public EmailAuditLoggerDecorator(
        IAuditLogger wrappedLogger,
        IEmailService emailService) 
        : base(wrappedLogger)
    {
        _emailService = emailService;
    }

    public override async Task LogAsync(AuditLogEntry logEntry)
    {
        // Send email for critical actions
        if (IsCriticalAction(logEntry))
        {
            await _emailService.SendAlertAsync(
                $"Critical Action: {logEntry.Action}",
                FormatEmailBody(logEntry)
            );
        }

        // Call wrapped logger
        await base.LogAsync(logEntry);
    }
    
    private bool IsCriticalAction(AuditLogEntry entry)
    {
        return entry.Action.Contains("DELETE") || 
               entry.Status == "FAILED";
    }
}
```

**Register**:
```csharp
logger = new EmailAuditLoggerDecorator(logger, emailService);
```

## 📝 Configuration Scenarios

### Scenario 1: Development (Full Logging)

```csharp
IAuditLogger logger = new DatabaseAuditLogger(dbContext);
logger = new PerformanceAuditLoggerDecorator(logger, 100);
logger = new FileAuditLoggerDecorator(logger);
logger = new ConsoleAuditLoggerDecorator(logger, true);
```

### Scenario 2: Production (Database + File only)

```csharp
IAuditLogger logger = new DatabaseAuditLogger(dbContext);
logger = new FileAuditLoggerDecorator(logger);
```

### Scenario 3: Testing (Console only)

```csharp
IAuditLogger logger = new DatabaseAuditLogger(dbContext);
logger = new ConsoleAuditLoggerDecorator(logger, true);
```

### Scenario 4: No Decorators (Database only)

```csharp
IAuditLogger logger = new DatabaseAuditLogger(dbContext);
```

## 🚀 Performance Considerations

1. **Decorator Order**: Đặt decorators nhanh trước, chậm sau
   - ✅ Console → File → Database
   - ❌ Database → File → Console

2. **Async Operations**: Tất cả decorators đều async
   - Không block main thread
   - Sử dụng `await` properly

3. **Error Handling**: Mỗi decorator có try-catch riêng
   - Lỗi ở decorator không ảnh hưởng chain
   - Log errors nhưng vẫn tiếp tục

## 📚 Related Patterns

1. **Repository Pattern**: Database access
2. **Dependency Injection**: Service registration
3. **Chain of Responsibility**: Decorator chain execution
4. **Strategy Pattern**: Swappable logging strategies

## 🔍 Monitoring & Debugging

### Enable Debug Mode

```csharp
logger = new ConsoleAuditLoggerDecorator(logger, enableColors: true);
logger = new PerformanceAuditLoggerDecorator(logger, warningThresholdMs: 100);
```

### Check Log Files

```bash
# View today's logs
cat Logs/Audit/audit_20251106.log

# Monitor in real-time
tail -f Logs/Audit/audit_20251106.log
```

### Query Database

```sql
SELECT * FROM AuditLog 
WHERE CreatedAt >= CURDATE() 
ORDER BY CreatedAt DESC;
```

## 📖 References

- Design Patterns: Elements of Reusable Object-Oriented Software (Gang of Four)
- Clean Architecture by Robert C. Martin
- ASP.NET Core Best Practices

---

**Last Updated**: November 6, 2025  
**Component**: Core/Infrastructure/Logging  
**Pattern**: Decorator Pattern  
**Architecture**: Component-Based Architecture
