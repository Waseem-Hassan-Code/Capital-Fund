using Capital.Funds.Models;

namespace Capital.Funds.Services.IServices
{
    public interface IUserEssentials
    {
        string LastException { get; }
        Task<string> getTenantIdAsync(string userId);
        Task<TenantPayments> getMontlyRentAsync(string userId);
        Task<IEnumerable<TenantComplaints>> getComplaintsAsync(string tenantId);
        Task<string> addComplaintAsync(TenantComplaints complaint);
        Task<PaginatedResult<TenantPayments>> getPaymentsHistoryAsync(string tenantId, int page, int pageSize);
    }
}
