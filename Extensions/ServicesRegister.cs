using ERP_API.Authorization;
using ERP_API.Entities;
using ERP_API.Repositories;
using ERP_API.Repositories.IRepositories;
using ERP_API.Repositories.tRepositories;
using ERP_API.Services;
using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Authorization;

namespace ERP_API.Extensions
{
    public static class ServicesRegister
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            //services.RegisterMapsterConfiguration();

            services.AddScoped<IUnitOfWork, UnitOfWork<ErpDbContext>>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            //Account
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            //Role
            services.AddScoped<IRoleService, RoleService>();

            //Customer
            services.AddTransient<Repositories.IRepositories.ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();

            //Supplier
            services.AddTransient<ISupplierRepository, Repositories.SupplierRepository>();
            services.AddScoped<ISupplierService, SupplierService>();

            //Category
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            //Warehouse
            services.AddTransient<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IWarehouseService, WarehouseService>();

            // Sale Orders
            services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
            services.AddTransient<ISalesOrderService, SalesOrderService>();

            // Purchase Orders
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddTransient<IPurchaseOrderService, PurchaseOrderService>();

            // Customers 2
            services.AddScoped<tICustomerRepository, tCustomerRepository>();

            // Supplier 2
            services.AddScoped<tISupplierRepository, tSupplierRepository>();

            // Products
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();

            services.AddTransient<IWarehouseReportRepository, WarehouseReportRepository>();
            services.AddScoped<IWarehouseReportService, WarehouseReportService>();
            // Purchase Staff
            services.AddScoped<IPurchaseStaffRepository, PurchaseStaffRepository>();
            services.AddTransient<IPurchaseStaffService, PurchaseStaffService>();

            // Sale Staff
            services.AddScoped<ISaleStaffRepository, SaleStaffRepository>();
            services.AddTransient<ISaleStaffService, SaleStaffService>();

            // Store
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddTransient<IStoreService, StoreService>();

            // Stock Transaction
            services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
            services.AddTransient<IStockTransactionService, StockTransactionService>();

            //Employees
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            //Department
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentService, DepartmentService>();

            // Audit Log
            services.AddScoped<IAuditLogService, AuditLogService>();
        }
    }
}
