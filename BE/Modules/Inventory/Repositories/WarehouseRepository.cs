using ERP_API.Utilities;
using Dapper;
using ERP_API.Core.Database.Entities;
using ERP_API.Models;
using ERP_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using System;
using System.Text;

namespace ERP_API.Repositories
{
    public class WarehouseRepository : BaseRepository<Warehouse, ErpDbContext>, IWarehouseRepository
    {
        public WarehouseRepository(ErpDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task<IEnumerable<WarehouseModel>> GetListPaging(WarehouseSearchModel search)
        {
            if (search == null)
            {
                return null;
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"SELECT w.WarehouseID, w.WarehouseName, w.Location FROM Warehouse w WHERE 1=1 ");

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query.Append(" AND (w.WarehouseName LIKE @keyword OR w.Location LIKE @keyword) ");
                dynamicParameters.Add("keyword", "%" + search.Keyword + "%");
            }

            query.Append(" ORDER BY w.WarehouseID DESC ");
            StringUtils.AddPaging(query, search.PageIndex, search.PageSize);

            IEnumerable<WarehouseModel> lstStudent = await DapperQueryAsync<WarehouseModel>(query.ToString(), dynamicParameters);

            return lstStudent;
        }

        public async Task<int> GetTotalRecord(WarehouseSearchModel search)
        {
            if (search == null)
            {
                return 0;
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"SELECT COUNT(1) FROM Warehouse w WHERE 1=1 ");

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query.Append(" AND (w.WarehouseName LIKE @keyword OR w.Location LIKE @keyword) ");
                dynamicParameters.Add("keyword", "%" + search.Keyword + "%");
            }

            var count = await DapperGetAsync<int>(query.ToString(), dynamicParameters);

            return count;
        }
    }
}
