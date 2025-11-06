using ERP_API.Utilities;
using Dapper;
using ERP_API.Core.Database.Entities;
using ERP_API.Models;
using ERP_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using System;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ERP_API.Repositories
{
    public class CategoryRepository : BaseRepository<Category, ErpDbContext>, ICategoryRepository
    {
        public CategoryRepository(ErpDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<IEnumerable<CategoryModel>> GetListPaging(CategorySearchModel search)
        {
            if (search == null)
            {
                return null;
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"SELECT c.CategoryID, c.CategoryName, c.Description FROM Category c WHERE 1=1 ");

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query.Append(" AND (c.CategoryName LIKE @keyword OR c.Description LIKE @keyword) ");
                dynamicParameters.Add("keyword", "%" + search.Keyword + "%");
            }

            query.Append(" ORDER BY c.CategoryID DESC ");
            StringUtils.AddPaging(query, search.PageIndex, search.PageSize);

            IEnumerable<CategoryModel> lstStudent = await DapperQueryAsync<CategoryModel>(query.ToString(), dynamicParameters);

            return lstStudent;
        }

        public async Task<int> GetTotalRecord(CategorySearchModel search)
        {
            if (search == null)
            {
                return 0;
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"SELECT COUNT(1) FROM Category c WHERE 1=1 ");

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query.Append(" AND (c.CategoryName LIKE @keyword OR c.Description LIKE @keyword) ");
                dynamicParameters.Add("keyword", "%" + search.Keyword + "%");
            }

            var count = await DapperGetAsync<int>(query.ToString(), dynamicParameters);

            return count;
        }
    }
}
