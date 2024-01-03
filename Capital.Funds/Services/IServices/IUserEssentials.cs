using Capital.Funds.Models;

namespace Capital.Funds.Services.IServices
{
    public interface IUserEssentials
    {
        string LastException { get; }
        Task<TenantPayments> getMontlyRentAsync(string tenantId);
        Task<IEnumerable<TenantComplaints>> getComplaintsAsync(string tenantId);
        Task<string> addComplaintAsync(TenantComplaints complaint);
        Task<PaginatedResult<TenantPayments>> getPaymentsHistoryAsync(string tenantId, int page, int pageSize);
    }
}
