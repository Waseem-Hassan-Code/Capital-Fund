using Capital.Funds.Models;
using Capital.Funds.Models.DTO;

namespace Capital.Funds.Services.IServices
{
    public interface ITenatsResidencyInfo
    {
        string LastException { get; }
        Task<TenatDetails> assignPropertyAsync(TenatDetails tenantDetails);
        Task<TenatDetails> updateAssignedPropertyAsync(TenatDetails tenantPayments);
        Task<string> deleteAssignedProperty(string contractId);
        Task<PaginatedResult<TenantsResidencyInfoDto>> getAllContracts(int page ,  int pageSize);
        Task<TenatDetails> getByIdAsync(string contractId);

    }
}
