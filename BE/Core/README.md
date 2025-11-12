# Core Component

## Mô tả

Core component chứa các thành phần dùng chung cho toàn bộ hệ thống ERP.

## Cấu trúc

### Common

- **Models**: Các model dùng chung (ResponseData, PageList, BaseSearch, etc.)
- **Enums**: Các enum định nghĩa (ErrorCodeAPI, PermissionEnum)
- **Utilities**: Các utility classes và extension methods

### Database

- **Entities**: Các entity classes (Domain models)
- **ErpDbContext**: Database context
- **BaseRepository**: Base repository pattern
- **UnitOfWork**: Unit of Work pattern

### Infrastructure

- **Authorization**: Permission-based authorization
- **Authentication**: Cookie-based authentication
- **Logging**: Audit logs
- **Middleware**: Custom middleware

## Dependency

Core component KHÔNG phụ thuộc vào bất kỳ module nào khác.
Tất cả các modules khác đều phụ thuộc vào Core.
