using ERP_API.Entities;
using ERP_API.Repositories;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services;
using ERP_API.Services.IServices;
using ERP_API.tRepositories;

namespace ERP_API.Extensions
{
    public static class ServicesRegister
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            //services.RegisterMapsterConfiguration();

            services.AddScoped<IUnitOfWork, UnitOfWork<ErpDbContext>>();

            //Customer
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();

            //Supplier
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ISupplierService, SupplierService>();

            //Category
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            //Warehouse
            services.AddTransient<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IWarehouseService, WarehouseService>();

            // Orders
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderService, OrderService>();

            // Customers 2
            services.AddScoped<tICustomerRepository, tCustomerRepository>();

            // Products
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();

        }
    }
}
