using ERP_API.Entities;
using ERP_API.Repositories;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services;
using ERP_API.Services.IServices;

namespace ApiWithRoles.Extensions
{
    public static class ServicesRegister
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            //services.RegisterMapsterConfiguration();

            services.AddScoped<IUnitOfWork, UnitOfWork<ErpDbContext>>();

            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
        }
    }
}
