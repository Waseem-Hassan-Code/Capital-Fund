using Capital.Funds.Models;

namespace Capital.Funds.Services.IServices
{
    public interface ITenantsComplains
    {
        string LastException { get; }
        Task<PaginatedResult<TenantComplaints>> GetTenantsComplainsAsync(int page, int pageSize);
        Task<bool> ChangeStatusAsync(string complainId);
        Task<bool> RemoveComplainAsync(string complainId);
    }
}
