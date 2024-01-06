using AutoMapper;
using Capital.Funds.Database;
using Capital.Funds.Models;
using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace Capital.Funds.Services
{
    public class ManageTenants : IManageTenants
    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;
        public string LastException { get; private set; }

        public ManageTenants(ApplicationDb db , IMapper mapper) {
            _db = db; 
            _mapper = mapper;
            LastException = null;
        }

        public async Task<string> addNewTenantAsync(Users user)
        {
            try
            {
                LastException = null;
                Users existingUser = await _db.Users.FirstOrDefaultAsync(u=>u.Email == user.Email.ToLower());

                if (existingUser != null)
                {
                    return SD.AlreadyRegistered;
                }

                string Salt = SD.GenerateSalt();
                string HashedPassword = SD.HashPassword(user.Password, Salt);

                Users newUser = new Users
                {
                    Id  = Guid.NewGuid().ToString("N"),
                    Name = user.Name,
                    Email = user.Email.ToLower(),
                    Password = HashedPassword,
                    Salt = Salt,
                    Gender = user.Gender,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    OTP = "",
                    isEmailVerified = user.isEmailVerified
                };
                await _db.AddAsync(newUser);
                int rows = await _db.SaveChangesAsync();

                if (rows > 0)
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

        public async Task<PaginatedResult<TenantPersonalInfoDto>> getAllTenantsAsync(int page , int pageSize)
        {
            try
            {
                LastException = null;
                var totalCount = await _db.Users.CountAsync();
                var tenants = await _db.Users
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (tenants.Any())
                {
                    IEnumerable<TenantPersonalInfoDto> tenantsList = _mapper.Map<IEnumerable<TenantPersonalInfoDto>>(tenants);

                    var paginatedResult = new PaginatedResult<TenantPersonalInfoDto>
                    {
                        Items = tenantsList,
                        TotalCount = totalCount,
                        Page = page,
                        PageSize = pageSize
                    };

                    return paginatedResult;
                }

                return new PaginatedResult<TenantPersonalInfoDto>
                {
                    Items = Enumerable.Empty<TenantPersonalInfoDto>(),
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

        public async Task<string> updateTenantsPersonalInfoAsync(TenantPersonalInfoDto personalInfo)
        {
            try
            {
                LastException = null;
                Users existingUser = await _db.Users.FindAsync(personalInfo.Id);

                if (existingUser == null)
                {
                    return SD.UserNotFound;
                }

                existingUser.Name = personalInfo.Name;
                existingUser.Email = personalInfo.Email;
                existingUser.Gender = personalInfo.Gender;
                existingUser.Role = personalInfo.Role;
                existingUser.IsActive = personalInfo.IsActive;
                existingUser.isEmailVerified = personalInfo.isEmailVerified;

                if (!string.IsNullOrEmpty(personalInfo.Password))
                {
                    string newSalt = SD.GenerateSalt();
                    existingUser.Salt = newSalt;
                    existingUser.Password = SD.HashPassword(personalInfo.Password, newSalt);
                }

                int rows = await _db.SaveChangesAsync();

                if (rows > 0)
                    return SD.RecordUpdated;
                else
                    return SD.RecordNotUpdated;
            }
            catch (Exception ex)
            {
                LastException = $"{ex.Message}";
            }
            return null;
        }



    }
}
