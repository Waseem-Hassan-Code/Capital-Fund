using Capital.Funds.Models;
using Capital.Funds.Models.DTO;

namespace Capital.Funds.Services.IServices
{
    public interface ITenantsComplains
    {
        string LastException { get; }
        Task<PaginatedResult<ComplaintsDTO>> GetTenantsComplainsAsync(int page, int pageSize);
        Task<bool> ChangeStatusAsync(string complainId, bool CompStatus);
        Task<bool> RemoveComplainAsync(string complainId);
    }
}
