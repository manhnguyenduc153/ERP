# Identity Module

## Mô tả

Module quản lý Authentication và Authorization cho hệ thống ERP.

## Chức năng chính

- 🔐 User Authentication (Login/Register)
- 👥 Role Management
- 🔑 Permission Management
- � Cookie-based Session Management
- 📝 User Profile Management

## Components

### Controllers

- **AccountsController**: Đăng nhập, đăng ký, quản lý user
- **RoleController**: Quản lý roles và permissions

### Services

- **AccountService**: Business logic cho user authentication
- **RoleService**: Business logic cho role management
- **AccountRepository**: Data access cho user/account

### DTOs

- Login, Register models
- RoleModel, RolePermissionModel
- UserRole, AccountModel

## Dependencies

- Core.Common (Models, Enums)
- Core.Database (Entities, DbContext)
- Core.Infrastructure (Authorization)
- ASP.NET Core Identity
- Cookie Authentication
