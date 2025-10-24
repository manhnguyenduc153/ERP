using ERP_API.Entities;
using ERP_API.Repositories;
using ERP_API.Repositories.IRepositories;
using ERP_API.Repositories.tRepositories;
using ERP_API.Services;
using ERP_API.Services.IServices;

namespace ERP_API.Extensions
{
    public static class ServicesRegister
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            //services.RegisterMapsterConfiguration();

            services.AddScoped<IUnitOfWork, UnitOfWork<ErpDbContext>>();

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

        }
    }
}
