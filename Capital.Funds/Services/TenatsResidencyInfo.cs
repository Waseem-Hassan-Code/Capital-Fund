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

        public TenatsResidencyInfo(ApplicationDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<TenatDetails> assignPropertyAsync(TenatDetails TenatDetails)
        {
            try
            {
                _mapper.Map(TenatDetails, TenatDetails);

                
                await _db.TenatDetails.AddAsync(TenatDetails);
                int count = await _db.SaveChangesAsync();

                if (count > 0)
                    return TenatDetails;

                return TenatDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> deleteAssignedProperty(string contractId)
        {
            try
            {
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
                return null;
            }
        }

        public async Task<PaginatedResult<TenatDetails>> getAllContracts(int page, int pageSize)
        {
            try
            {
                var tottalCount = await _db.TenatDetails.CountAsync();
                var details = await _db.TenatDetails
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (details!=null)
                {
                    IEnumerable<TenatDetails> tenantsList = _mapper.Map<IEnumerable<TenatDetails>>(details);
                    var paginatedResults = new PaginatedResult<TenatDetails>
                    {
                        Items = tenantsList,
                        TotalCount = tottalCount,
                        PageSize = pageSize,
                        Page = page
                    };

                    return paginatedResults;
                }

                return new PaginatedResult<TenatDetails>
                {
                    Items = Enumerable.Empty<TenatDetails>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TenatDetails> getByIdAsync(string contractId)
        {
            try
            {
                var search = await _db.TenatDetails.FirstOrDefaultAsync(x => x.Id == contractId);
                if (search != null)
                    return search;

                TenatDetails details = new TenatDetails();
                return details;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TenatDetails> updateAssignedPropertyAsync(TenatDetails tenantPayments)
        {
            try
            {
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
                return null;
            }
        }
    }
}
