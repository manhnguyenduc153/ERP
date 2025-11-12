# Reporting Module

## Mô tả

Module quản lý báo cáo và phân tích dữ liệu.

## Chức năng chính

- 📊 Sales Analytics & Reports
- 📦 Inventory Reports
- 💰 Financial Reports
- 👥 HR Reports
- 📈 Dashboard KPIs

## Components

### Controllers

- **ReportController**: API endpoints cho reports (Future)

### Services

- **ReportService**: Business logic cho reporting (Future)

### DTOs

- ReportStatistic DTOs

## Dependencies

- Core.Common (Models, Enums)
- Core.Database (Entities, DbContext)
- ALL Business Modules (để lấy data)

## Future Enhancements

- Real-time dashboard
- Custom report builder
- Export to Excel/PDF
- Scheduled reports
- Email notifications
