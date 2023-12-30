using AutoMapper;
using Capital.Funds.Database;
using Capital.Funds.Models;
using Capital.Funds.Services.IServices;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Capital.Funds.Services
{
    public class TenantsComplains : ITenantsComplains
    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;

        public TenantsComplains(ApplicationDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> RemoveComplainAsync(string complainId)
        {
            try{
                string sqlQuery = $"DELETE FROM TenantComplaints WHERE Id = @ComplainId";
                int rowsAffected = await _db.Database.ExecuteSqlRawAsync(sqlQuery, new SqliteParameter("@ComplainId", complainId));

                if (rowsAffected == 0)
                    return false;
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> ChangeStatusAsync(string complainId)
        {
            try
            {
                string sqlQuery = $"UPDATE TenantComplaints SET IsFixed = 1 WHERE Id = @ComplainId";
                int rowsAffected = await _db.Database.ExecuteSqlRawAsync(sqlQuery, new SqliteParameter("@ComplainId", complainId));

                if (rowsAffected == 0 )
                    return false;
                return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<PaginatedResult<TenantComplaints>> GetTenantsComplainsAsync(int page , int pageSize)
        {
            try
            {
                var tottalCount = await _db.TenantComplaints.CountAsync();
                var details = await _db.TenatDetails
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (details!=null)
                {
                    IEnumerable<TenantComplaints> complainList = _mapper.Map<IEnumerable<TenantComplaints>>(details);
                    var paginatedResults = new PaginatedResult<TenantComplaints>
                    {
                        Items = complainList,
                        TotalCount = tottalCount,
                        PageSize = pageSize,
                        Page = page
                    };

                    return paginatedResults;
                }

                return new PaginatedResult<TenantComplaints>
                {
                    Items = Enumerable.Empty<TenantComplaints>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
