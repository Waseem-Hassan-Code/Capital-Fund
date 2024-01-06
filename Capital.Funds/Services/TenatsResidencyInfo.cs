using AutoMapper;
using Capital.Funds.Database;
using Capital.Funds.Models;
using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.EntityFrameworkCore;

namespace Capital.Funds.Services
{
    public class TenatsResidencyInfo : ITenatsResidencyInfo
    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;
        public string LastException { get; private set; }

        public TenatsResidencyInfo(ApplicationDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            LastException = null;
        }
        public async Task<TenatDetails> assignPropertyAsync(TenatDetails TenatDetails)
        {
            try
            {
                LastException = null;
                TenatDetails newTenant = new TenatDetails
                {
                    Id = Guid.NewGuid().ToString("N"),
                    UserId = TenatDetails.UserId,
                    PropertyId = TenatDetails.PropertyId,
                    MovedIn = TenatDetails.MovedIn,
                    MovedOut = ""
                };

                await _db.TenatDetails.AddAsync(newTenant);
                int count = await _db.SaveChangesAsync();

                if (count > 0)
                    return TenatDetails;

                return TenatDetails;
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }

        public async Task<string> deleteAssignedProperty(string contractId)
        {
            try
            {
                LastException = null;
                var search = await _db.TenatDetails.FirstOrDefaultAsync(t=>t.Id == contractId);
                if (search != null)
                {
                     _db.TenatDetails.Remove(search);
                    int rows = await _db.SaveChangesAsync();

                    if (rows > 0)
                        return SD.RecordUpdated;    
                }
                return SD.RecordNotUpdated;
            }
            catch(Exception ex)
            {
                LastException=ex.Message;
            }
            return null;
        }

        public async Task<PaginatedResult<TenantsResidencyInfoDto>> getAllContracts(int page, int pageSize)
        {
            try
            {
                LastException = null;
                var tottalCount = await _db.TenatDetails.CountAsync();

                var details = await (
                    from residence in _db.TenatDetails
                    join user in _db.Users on residence.UserId equals user.Id
                    join prop in _db.PropertyDetails on residence.PropertyId equals prop.Id
                    select new TenantsResidencyInfoDto
                    {
                        Id = residence.Id,
                        UserName = user.Name,
                        PropertyName = prop.PropertyName,
                        MovedIn = residence.MovedIn,
                        MovedOut = residence.MovedOut,
                    })
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();


                if (details!=null)
                {
                    //IEnumerable<TenantsResidencyInfoDto> tenantsList = _mapper.Map<IEnumerable<TenantsResidencyInfoDto>>(details);
                    var paginatedResults = new PaginatedResult<TenantsResidencyInfoDto>
                    {
                        Items = details,
                        TotalCount = tottalCount,
                        PageSize = pageSize,
                        Page = page
                    };

                    return paginatedResults;
                }

                return new PaginatedResult<TenantsResidencyInfoDto>
                {
                    Items = Enumerable.Empty<TenantsResidencyInfoDto>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };

            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }

        public async Task<TenatDetails> getByIdAsync(string contractId)
        {
            try
            {
                LastException = null;
                var search = await _db.TenatDetails.FirstOrDefaultAsync(x => x.Id == contractId);
                if (search != null)
                    return search;

                TenatDetails details = new TenatDetails();
                return details;
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }

        public async Task<TenatDetails> updateAssignedPropertyAsync(TenatDetails tenantPayments)
        {
            try
            {
                LastException = null;
                TenatDetails updateDetails = await _db.TenatDetails.FirstOrDefaultAsync(u => u.Id == tenantPayments.Id);
                if (updateDetails != null)
                {
                    _mapper.Map(tenantPayments, updateDetails);
                    int rows = await _db.SaveChangesAsync();

                    if (rows > 0)
                        return updateDetails;
                }
                return updateDetails;
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }
    }
}
