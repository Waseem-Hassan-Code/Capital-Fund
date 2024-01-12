using Capital.Funds.Models;

namespace Capital.Funds.Services.IServices
{
    public interface IUserEssentials
    {
        string LastException { get; }
        Task<string> getTenantIdAsync(string userId);
        Task<TenantPayments> getMontlyRentAsync(string userId);
        Task<PaginatedResult<TenantComplaints>> getComplaintsAsync(string tenantId, int page, int pageSize);
        Task<string> addComplaintAsync(TenantComplaints complaint, IFormFile file);
        Task<PaginatedResult<TenantPayments>> getPaymentsHistoryAsync(string tenantId, int page, int pageSize);
    }
}
