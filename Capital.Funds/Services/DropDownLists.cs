using AutoMapper;
using Capital.Funds.Database;
using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Capital.Funds.Services
{
    public class DropDownLists : IDropDownLists
    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;
        public string LastException { get; private set; }

        public DropDownLists(ApplicationDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            LastException = null;
        }
        public async Task<IEnumerable<DDLTenantName>> DDLTenantNames()
        {
            try
            {
                LastException = null;
                var list = await (
                    from Tenant in _db.TenatDetails
                    join User in _db.Users on Tenant.UserId equals User.Id
                    select new
                    {
                        TenantId = Tenant.Id,
                        TenantName = User.Name
                    }
                    ).ToListAsync();

                if (list!=null)
                {
                    IEnumerable<DDLTenantName> names = _mapper.Map<IEnumerable<DDLTenantName>>(list);
                    return names;
                }
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            IEnumerable<DDLTenantName> emptyList = new List<DDLTenantName>();
            return emptyList;
        }

        public async Task<IEnumerable<DDLUserName>> DDLUserNames()
        {
            try
            {
                LastException = null;
                var list = await(
                    from User in _db.Users
                    select new
                    {
                        UserId = User.Id,
                        UserName = User.Name
                    }
                    ).ToListAsync();

                if (list!=null)
                {
                    IEnumerable<DDLUserName> names = _mapper.Map<IEnumerable<DDLUserName>>(list);
                    return names;
                }
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            IEnumerable<DDLUserName> emptyList = new List<DDLUserName>();
            return emptyList;
        }

        public async Task<IEnumerable<DDLPropertyName>> DDLPropertyNames()
        {
            try
            {
                LastException = null;
                var list = await(
                    from prop in _db.PropertyDetails
                    select new
                    {
                        UserId = prop.Id,
                        UserName = prop.PropertyName
                    }
                    ).ToListAsync();

                if (list!=null)
                {
                    IEnumerable<DDLPropertyName> names = _mapper.Map<IEnumerable<DDLPropertyName>>(list);
                    return names;
                }
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            IEnumerable<DDLPropertyName> emptyList = new List<DDLPropertyName>();
            return emptyList;
        }
    }
}
