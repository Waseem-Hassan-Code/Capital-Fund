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
        private readonly FileHandling _fileHandling;
        public string LastException { get; private set; }

        public UserEssentials(ApplicationDb db, FileHandling fileHandling)
        {
            _db = db;
            LastException = null;
            _fileHandling = fileHandling;
        }

        public async Task<string> addComplaintAsync(TenantComplaints complaint, IFormFile file)
        {
            try
            {
                using (var transaction = await _db.Database.BeginTransactionAsync())
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
                            await transaction.CommitAsync();
                            string ComplaintId = complaints.Id;

                            string fileUpload = await _fileHandling.UploadFileAsync(file, ComplaintId);
                            if(fileUpload== "Failed to save file information to the database" || fileUpload=="Error"||fileUpload== "File not uploaded") 
                            {
                                throw new Exception();
                            }

                            LastException = null;
                            return SD.RecordUpdated;
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        LastException = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return SD.RecordNotUpdated; 
        }

        public async Task<PaginatedResult<TenantComplaints>> getComplaintsAsync(string tenantId, int page , int pageSize)
        {
            try
            {
                var tenant = await _db.TenatDetails.FirstOrDefaultAsync(t=>t.UserId == tenantId);
                var results = await _db.TenantComplaints
                    .Where(t => t.TenantId == tenant.Id)
                    .OrderByDescending(t => t.CreatedAt)
                    .Skip(page - 1)
                    .Take(pageSize)
                    .ToListAsync();

                int totalCount = results.Count;
                if (results.Any())
                {
                    IEnumerable<TenantComplaints> list = results;
                    PaginatedResult<TenantComplaints> paginatedResult = new PaginatedResult<TenantComplaints>
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
                LastException = ex.Message;
            }
            return null;
        }

        public async Task<TenantPayments> getMontlyRentAsync(string userId)
        {
            try
            {
                var tenant = await _db.TenatDetails.Where(u => u.UserId == userId).FirstOrDefaultAsync();
                var payment = await _db.TenantPayments
                   .Where(t => t.TenantId == tenant.Id && t.isPayable == true)
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
                var tenant = await _db.TenatDetails.Where(u => u.UserId == tenantId).FirstOrDefaultAsync();
                var results = await _db.TenantPayments.Where(r=>r.TenantId == tenant.Id)
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

        public async Task<string> getTenantIdAsync(string userId)
        {
            try{
                LastException = null;

                string tenantId = await _db.TenatDetails
                                 .Where(u => u.UserId == userId)
                                 .Select(u => u.Id)
                                 .FirstOrDefaultAsync();

                if (tenantId != null)
                    return tenantId;
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }
    }
}
