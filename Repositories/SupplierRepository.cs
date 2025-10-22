using ERP_API.Utilities;
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
    public class SupplierRepository : BaseRepository<Supplier, ErpDbContext>, ISupplierRepository
    {
        public SupplierRepository(ErpDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task<IEnumerable<SupplierModel>> GetListPaging(SupplierSearchModel search)
        {
            if (search == null)
            {
                return null;
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"SELECT s.SupplierId, s.SupplierName, s.Contact, s.Address FROM Supplier s WHERE 1=1 ");

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query.Append(" AND (s.SupplierName LIKE @keyword OR s.Contact LIKE @keyword OR s.Address LIKE @keyword) ");
                dynamicParameters.Add("keyword", "%" + search.Keyword + "%");
            }

            query.Append(" ORDER BY s.SupplierId DESC ");
            StringUtils.AddPaging(query, search.PageIndex, search.PageSize);

            IEnumerable<SupplierModel> lstStudent = await DapperQueryAsync<SupplierModel>(query.ToString(), dynamicParameters);

            return lstStudent;
        }

        public async Task<int> GetTotalRecord(SupplierSearchModel search)
        {
            if (search == null)
            {
                return 0;
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"SELECT COUNT(1) FROM Supplier s WHERE 1=1 ");

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query.Append(" AND (s.SupplierName LIKE @keyword OR s.Contact LIKE @keyword OR s.Address LIKE @keyword) ");
                dynamicParameters.Add("keyword", "%" + search.Keyword + "%");
            }

            var count = await DapperGetAsync<int>(query.ToString(), dynamicParameters);

            return count;
        }
    }
}
