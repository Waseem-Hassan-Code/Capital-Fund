using AutoMapper;
using Capital.Funds.Database;
using Capital.Funds.Models;
using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Capital.Funds.Services
{
    public class BuildingMangment : IBuildingManagment
    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;
        public string LastException { get; private set; }

        public BuildingMangment(ApplicationDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            LastException = null;
        }
        public async Task<string> createPropertyAsync(PropertyDetails property)
        {
            try
            {
                PropertyDetails propertyDetails = new PropertyDetails()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    PropertyName = property.PropertyName,
                    Address = property.Address,
                    NumberofBedrooms = property.NumberofBedrooms,
                    NumberofBathrooms = property.NumberofBathrooms,
                    isAvailable = property.isAvailable,
                    TypeofProperty = property.TypeofProperty,
                    Description = property.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                 await _db.AddAsync(propertyDetails);
                int count = await _db.SaveChangesAsync();

                if (count > 0)
                {
                    LastException = null;
                    return SD.RecordUpdated;
                }
                else
                    return SD.RecordNotUpdated;
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }

        public async Task<PaginatedResult<PropertyDetails>> getPropertyAsync(int page, int pageSize)
        {
            try
            {
                LastException = null;
                var totalCount =await _db.PropertyDetails.CountAsync();
                var properties = await _db.PropertyDetails
                    .Skip(page-1)
                    .Take(pageSize)
                    .ToListAsync();

                if (properties.Any())
                {
                    IEnumerable<PropertyDetails> prop = _mapper.Map<IEnumerable<PropertyDetails>>(properties);

                    var paginatedResult = new PaginatedResult<PropertyDetails>
                    {
                        Items = prop,
                        TotalCount = totalCount,
                        Page = page,
                        PageSize = pageSize
                    };
                    return paginatedResult;
                }

                return new PaginatedResult<PropertyDetails>
                {
                    Items = Enumerable.Empty<PropertyDetails>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };
            }
            catch(Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }

        public async Task<PropertyDetails> getPropertyByIdAsync(string propertyId)
        {
            try
            {
                LastException = null;
                var obj = await _db.PropertyDetails.FirstOrDefaultAsync(p => p.Id == propertyId);
                if (obj == null)
                    return new PropertyDetails();
                return obj;

            }
            catch(Exception ex) {
                LastException = ex.Message;
            }
            return null;
        }

        public async Task<string> removePropertyAsync(string propertyId)
        {
            try
            {
                LastException = null;
                var obj = await _db.PropertyDetails.FirstOrDefaultAsync(p => p.Id == propertyId);
                if (obj == null)
                    return SD.UserNotFound;

               _db.PropertyDetails.Remove(obj);
               int rows = await _db.SaveChangesAsync();

               if (rows > 0)
                    return SD.RecordUpdated;
                
                return SD.RecordNotUpdated;
            }
            catch(Exception ex)
            {
                LastException= ex.Message;
            }
            return null;
        }

        public async Task<string> updatePropertyAsync(PropertyDetails property)
        {
            try
            {
                LastException = null;
                var existingProperty = await _db.PropertyDetails.FindAsync(property.Id);

                if (existingProperty == null)
                {
                    return SD.UserNotFound;
                    
                }

                existingProperty.PropertyName = property.PropertyName;
                existingProperty.Address = property.Address;
                existingProperty.NumberofBedrooms = property.NumberofBedrooms;
                existingProperty.NumberofBathrooms = property.NumberofBathrooms;
                existingProperty.isAvailable = property.isAvailable;
                existingProperty.Description = property.Description;
                existingProperty.UpdatedAt = DateTime.Now;

                int count = await _db.SaveChangesAsync();

                if (count > 0)
                    return SD.RecordUpdated;
                else
                    return SD.RecordNotUpdated;
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }


    }
}
