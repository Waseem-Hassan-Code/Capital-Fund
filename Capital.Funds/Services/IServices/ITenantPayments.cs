using Capital.Funds.Models;

namespace Capital.Funds.Services.IServices
{
    public interface ITenantPayments
    {
        string LastException { get; }
        Task<Models.TenantPayments> addTenantPaymentAsync(Models.TenantPayments tenantPayments);
        Task<Models.TenantPayments> updateTenantPaymentAsyn(Models.TenantPayments tenantPayments);
        Task<string> deleteTenantPaymentAsync(string contractId);
        Task<PaginatedResult<Models.TenantPayments>> getAllTenatPaymentsAsync(int page, int pageSize);
        Task<PaginatedResult<Models.TenantPayments>> getTenantPaymentByIdAsync(int page, int pageSize, string TenantId);
    }
}
