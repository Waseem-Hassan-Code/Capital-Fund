using Capital.Funds.Models;

namespace Capital.Funds.Services.IServices
{
    public interface IBuildingManagment
    {
        string LastException { get; }
        Task<PaginatedResult<PropertyDetails>> getPropertyAsync(int page, int pageSize);
        Task<PropertyDetails> getPropertyByIdAsync(string propertyId);
        Task<string> updatePropertyAsync(PropertyDetails property);
        Task<string> createPropertyAsync(PropertyDetails property);
        Task<string> removePropertyAsync(string propertyId);

    }
}
