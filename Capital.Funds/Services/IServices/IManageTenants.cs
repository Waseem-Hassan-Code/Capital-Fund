using Capital.Funds.Models;
using Capital.Funds.Models.DTO;

namespace Capital.Funds.Services.IServices
{
    public interface IManageTenants
    {
        string LastException { get; }
        Task<PaginatedResult<TenantPersonalInfoDto>> getAllTenantsAsync(int page, int pageSize);
        Task <string> updateTenantsPersonalInfoAsync(TenantPersonalInfoDto personalInfo);
        Task <string> addNewTenantAsync(Users user);
    }
}
