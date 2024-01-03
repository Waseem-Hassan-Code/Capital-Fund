using Capital.Funds.Models;

namespace Capital.Funds.Services.IServices
{
    public interface ITenatsResidencyInfo
    {
        string LastException { get; }
        Task<TenatDetails> assignPropertyAsync(TenatDetails tenantDetails);
        Task<TenatDetails> updateAssignedPropertyAsync(TenatDetails tenantPayments);
        Task<string> deleteAssignedProperty(string contractId);
        Task<PaginatedResult<TenatDetails>> getAllContracts(int page ,  int pageSize);
        Task<TenatDetails> getByIdAsync(string contractId);

    }
}
