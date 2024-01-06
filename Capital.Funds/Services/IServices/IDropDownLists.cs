using Capital.Funds.Models.DTO;
using System.Threading.Tasks;

namespace Capital.Funds.Services.IServices
{
    public interface IDropDownLists
    {
        string LastException { get; }
        Task<IEnumerable<DDLTenantName>> DDLTenantNames();
        Task<IEnumerable<DDLUserName>> DDLUserNames();
        Task<IEnumerable<DDLPropertyName>> DDLPropertyNames();
    }
}
