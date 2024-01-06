using AutoMapper;
using Capital.Funds.Database;
using Capital.Funds.Models;
using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Capital.Funds.Services
{
    public class TenantsComplains : ITenantsComplains
    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;
        public string LastException { get; private set; }

        public TenantsComplains(ApplicationDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            LastException = null;
        }

        public async Task<bool> RemoveComplainAsync(string complainId)
        {
            try
            {
                LastException = null;
                string sqlQuery = $"DELETE FROM TenantComplaints WHERE Id = @ComplainId";
                int rowsAffected = await _db.Database.ExecuteSqlRawAsync(sqlQuery, new SqliteParameter("@ComplainId", complainId));

                if (rowsAffected == 0)
                    return false;
                return true;

            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return false;
        }
        public async Task<bool> ChangeStatusAsync(string complainId)
        {
            try
            {
                LastException = null;
                string sqlQuery = $"UPDATE TenantComplaints SET IsFixed = 1, SET UpdatedAt = GETDATE() WHERE Id = @ComplainId";
                int rowsAffected = await _db.Database.ExecuteSqlRawAsync(sqlQuery, new SqliteParameter("@ComplainId", complainId));

                if (rowsAffected == 0)
                    return false;
                return true;

            }
            catch (Exception ex)
            {
                LastException=ex.Message;
            }
            return false;
        }

        public async Task<PaginatedResult<ComplaintsDTO>> GetTenantsComplainsAsync(int page, int pageSize)
        {
            try
            {
                LastException = null;
                var tottalCount = await _db.TenantComplaints.CountAsync();

                var list = await (
                    from complaint in _db.TenantComplaints
                    join tenant in _db.TenatDetails on complaint.TenantId equals tenant.Id
                    join User in _db.Users on tenant.UserId equals User.Id
                       select new
                            {
                              ComplaintId = complaint.Id,
                              TenantName = User.Name,
                              ComplaintTitle = complaint.Title,
                              ComplaintDetails = complaint.Details,
                              isFixed = complaint.IsFixed,
                              ComplainDate = complaint.CreatedAt
                            })
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize).ToListAsync();

                if (list!=null)
                {
                    IEnumerable<ComplaintsDTO> results = _mapper.Map<IEnumerable<ComplaintsDTO>>(list);
                    var paginatedResults = new PaginatedResult<ComplaintsDTO>
                    {
                        Items = results,
                        TotalCount = tottalCount,
                        PageSize = pageSize,
                        Page = page
                    };
                    return paginatedResults;
                }

                return new PaginatedResult<ComplaintsDTO>
                {
                    Items = Enumerable.Empty<ComplaintsDTO>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };

            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }

        public async Task<IEnumerable<DDLTenantName>> DDLTenantNames()
        {
            try
            {
                LastException = null;
                var list  = await (
                    from Tenant in _db.TenatDetails 
                    join User in _db.Users on Tenant.UserId equals User.Id
                    select new 
                    { TenantId = Tenant.Id,
                      TenantName = User.Name}
                    ).ToListAsync();

                if (list!=null)
                {
                    IEnumerable<DDLTenantName> names = _mapper.Map<IEnumerable<DDLTenantName>>(list);
                    return names;
                }
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            IEnumerable<DDLTenantName> emptyList = new List<DDLTenantName>();
            return emptyList;
        }
    }
}
