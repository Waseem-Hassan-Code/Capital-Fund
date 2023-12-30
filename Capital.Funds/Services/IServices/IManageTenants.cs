using Capital.Funds.Models;
using Capital.Funds.Models.DTO;

namespace Capital.Funds.Services.IServices
{
    public interface IManageTenants
    {
        Task<PaginatedResult<TenantPersonalInfo>> getAllTenantsAsync(int page, int pageSize);
        Task <string> updateTenantsPersonalInfoAsync(TenantPersonalInfo personalInfo);
        Task <string> addNewTenantAsync(Users user);
    }
}
