# Human Resources Module

## Mô tả

Module quản lý nhân sự và tổ chức.

## Chức năng chính

- 👥 Employee Management (CRUD)
- 🏢 Department Management
- 💼 Position Management
- 💰 Payroll Management (Future)
- 📅 Attendance Tracking (Future)
- 📊 HR Analytics

## Components

### Controllers

- **EmployeeController**: Quản lý nhân viên
- **DepartmentController**: Quản lý phòng ban

### Services

- **EmployeeService**: Business logic cho employees
- **DepartmentService**: Business logic cho departments

### Repositories

- **EmployeeRepository**: Data access cho employees
- **DepartmentRepository**: Data access cho departments

### DTOs

- Employee DTOs (CreateEmployeeDto, UpdateEmployeeDto, EmployeeResponseDto)
- Department DTOs (CreateDepartmentDto, UpdateDepartmentDto, DepartmentResponseDto)

## Dependencies

- Core.Common (Models, Enums)
- Core.Database (Entities, DbContext, BaseRepository)
- Identity Module (User account linking)

## Future Enhancements

- Payroll processing
- Attendance tracking
- Leave management
- Performance reviews
- Training management
