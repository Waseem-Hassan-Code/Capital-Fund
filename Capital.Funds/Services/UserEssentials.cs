using Capital.Funds.Database;
using Capital.Funds.Models;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.EntityFrameworkCore;

namespace Capital.Funds.Services
{
    public class UserEssentials : IUserEssentials
    {
        private readonly ApplicationDb _db;
        public string LastException { get; private set; }

        public UserEssentials(ApplicationDb db)
        {
            _db=db;
            LastException = null;
        }

        public async Task<string> addComplaintAsync(TenantComplaints complaint)
        {
            try
            {
                TenantComplaints complaints = new TenantComplaints
                {
                    Id = Guid.NewGuid().ToString("N"),
                    TenantId = complaint.TenantId,
                    Title = complaint.Title,
                    Details = complaint.Details,
                    IsFixed = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                await _db.AddAsync(complaints);
                int count = await _db.SaveChangesAsync();

                if (count > 0)
                {
                    LastException = null;
                    return SD.RecordUpdated;
                }
                    
            }catch(Exception ex)
            {
                LastException = ex.Message;
            }
            return SD.RecordNotUpdated;
        }

        public async Task<IEnumerable<TenantComplaints>> getComplaintsAsync(string tenantId)
        {
            try
            {
                IEnumerable<TenantComplaints> complaints = await _db.TenantComplaints
                    .Where(t => t.TenantId == tenantId)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();

                if (complaints != null)
                {
                    LastException = null;
                    return complaints;
                }
                    
            }
            catch(Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }

        public async Task<TenantPayments> getMontlyRentAsync(string tenantId)
        {
            try
            {
                var payment = await _db.TenantPayments
                   .Where(t => t.TenantId == tenantId && t.isPayable == true)
                   .OrderByDescending(t => t.CreatedAt)
                   .FirstOrDefaultAsync();

                if (payment != null)
                {
                    LastException = null;
                    return payment;
                }
                    
            }
            catch(Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }

        public async Task<PaginatedResult<TenantPayments>> getPaymentsHistoryAsync(string tenantId, int page, int pageSize)
        {
            try
            {
                var results = await _db.TenantPayments.Where(r=>r.TenantId == tenantId)
                    .OrderByDescending(o=>o.RentPayedAt)
                    .Skip(page-1)
                    .Take(pageSize)
                    .ToListAsync();

                int totalCount = results.Count;
                if (results.Any())
                {
                    IEnumerable<TenantPayments> list = results;
                    PaginatedResult<TenantPayments> paginatedResult = new PaginatedResult<TenantPayments>
                    {
                        Items = list,
                        TotalCount = totalCount,
                        Page = page,
                        PageSize = pageSize
                    };
                    LastException = null;
                    return paginatedResult;
                }
            }
            catch(Exception ex)
            {
                LastException=ex.Message;
            }
            return null;
        }
    }
}
