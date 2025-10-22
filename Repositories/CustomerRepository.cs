using ApiWithRoles.Utilities;
using Dapper;
using ERP_API.Entities;
using ERP_API.Models;
using ERP_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using System;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ERP_API.Repositories
{
    public class CustomerRepository : BaseRepository<Customer, ErpDbContext>, ICustomerRepository
    {
        public CustomerRepository(ErpDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task<IEnumerable<CustomerModel>> GetListPaging(CustomerSearchModel search)
        {
            if (search == null)
            {
                return null;
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"SELECT * FROM Customer c WHERE 1=1 ");

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query.Append(" AND c.Name LIKE @keyword");
                dynamicParameters.Add("keyword", "%" + search.Keyword + "%");
            }

            query.Append(" ORDER BY c.CustomerId DESC ");
            StringUtils.AddPaging(query, search.PageIndex, search.PageSize);

            IEnumerable<CustomerModel> lstStudent = await DapperQueryAsync<CustomerModel>(query.ToString(), dynamicParameters);

            return lstStudent;
        }

        public async Task<int> GetTotalRecord(CustomerSearchModel search)
        {
            if (search == null)
            {
                return 0;
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"SELECT * FROM Customer c WHERE 1=1 ");

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query.Append(" AND c.Name LIKE @keyword");
                dynamicParameters.Add("keyword", "%" + search.Keyword + "%");
            }

            var count = await DapperGetAsync<int>(query.ToString(), dynamicParameters);

            return count;
        }
    }
}
